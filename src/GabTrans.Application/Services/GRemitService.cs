using Azure.Storage.Blobs.Models;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.GRemit;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace GabTrans.Application.Services
{
    public class GRemitService(ILogService logService, IGlobusBankService globusBankService, ITransferRepository transferRepository, IBankTransferService bankTransferService, IGRemitClientIntegration gRemitClientIntegration, IPlatformTransactionRepository platformTransferRepository) : IGRemitService
    {
        private readonly ILogService _logService = logService;
        private readonly IGlobusBankService _globusBankService = globusBankService;
        private readonly ITransferRepository _transferRepository = transferRepository;
        private readonly IBankTransferService _bankTransferService = bankTransferService;
        private readonly IGRemitClientIntegration _gRemitClientIntegration = gRemitClientIntegration;
        private readonly IPlatformTransactionRepository _platformTransferRepository = platformTransferRepository;

        public async Task NotifyAsync()
        {
            try
            {
                var applicationIds = StaticData.GremitAccounts.Where(x => string.Equals(x.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase)).DistinctBy(x => x.AccountId).Select(x => x.AccountId).ToList();

                var transfers = await _platformTransferRepository.GetAsync(GRemitStatuses.Paying, applicationIds);

                if (transfers.Any()) _logService.LogInfo("GRemitService", "NotifyAsync", $"Total number of payouts is : {transfers.Count()}");

                foreach (var transfer in transfers)
                {
                    try
                    {
                        var details = await _transferRepository.DetailsAsync(transfer.Reference);
                        if (details is null) continue;

                        if (string.Equals(details.Status, TransactionStatuses.Pending, StringComparison.OrdinalIgnoreCase)) continue;

                        string reason = details.FailureReason ?? "session timeout from the beneficiary bank";

                        var gremitApplication = StaticData.GremitAccounts.FirstOrDefault(x => x.AccountId == transfer.AccountId && x.Country == Countries.Nigeria);
                        if (gremitApplication is null) continue;

                        if (string.Equals(details.Status, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase)) await ApprovedAsync(gremitApplication, transfer.Reference);

                        if (string.Equals(details.Status, TransactionStatuses.Reversed, StringComparison.OrdinalIgnoreCase)) await RejectAsync(gremitApplication, transfer.Reference, reason);

                        if (string.Equals(details.Status, TransactionStatuses.Failed, StringComparison.OrdinalIgnoreCase)) await RejectAsync(gremitApplication, transfer.Reference, reason);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(GRemitService), nameof(NotifyAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(GRemitService), nameof(NotifyAsync), ex);
            }
        }

        public async Task ApprovedAsync(GremitAccount gremitApplication, string reference)
        {
            string updatedAt = DateTime.Now.ToString("yyyy-MM-dd");

            var response = await _gRemitClientIntegration.ApproveAsync(gremitApplication, reference, updatedAt);
            if (response is null || response.Result is null) return;

            if (response.Result.ResultCode != "1000") return;

            await _platformTransferRepository.UpdateStatusAsync(reference, GRemitStatuses.Paid, JsonConvert.SerializeObject(response));
        }

        public async Task RejectAsync(GremitAccount gremitApplication, string reference, string reason)
        {
            var response = await _gRemitClientIntegration.RejectAsync(gremitApplication, reference, reason);
            if (response is null || response.Result is null) return;

            if (response.Result.ResultCode != "1000") return;

            await _platformTransferRepository.UpdateStatusAsync(reference, GRemitStatuses.Rejected, JsonConvert.SerializeObject(response));
        }

        public async Task FetchAsync()
        {
            try
            {
                int counter = 0;

                var applications = await _platformTransferRepository.GetGRemitAsync(AccountStatuses.Active);
                foreach (var application in applications)
                {
                    bool processed = false;

                    if (string.Equals(application.DeliveryMethod, GRemitDeliveryMethods.Deposit, StringComparison.OrdinalIgnoreCase)) processed = await LogDepositAsync(application);
                    if (processed) counter++;
                }

                if (counter > 0) _logService.LogInfo("GRemitService", "FetchAsync", $"Processed {counter} applications");
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "FetchAsync", ex);
            }
        }


        public async Task<bool> LogDepositAsync(GremitAccount gremitApplication)
        {
            try
            {
                bool insert = false;

                var platformTransactions = new List<PlatformTransaction>();

                var response = await _gRemitClientIntegration.GetTransactionsAsync(gremitApplication);
                if (response is null || response.Result is null) return false;

                if (!response.Result.ResultCode.Equals("1000")) return false;

                if (response.Result.Details.Transaction.Count > 0) _logService.LogInfo("GRemitService", "LogDepositAsync", $"Total number of transactions retrieved from GRemit is : {response.Result.Details.Transaction.Count} for {gremitApplication.Name}");

                foreach (var transaction in response.Result.Details.Transaction)
                {
                    try
                    {
                        var platformDetails = await _platformTransferRepository.DetailsAsync(transaction.ReferenceNo);
                        if (platformDetails is not null)
                        {
                            _logService.LogInfo("GRemitService", "LogDepositAsync", $"Reference already exists on platform:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        platformTransactions.Add(new PlatformTransaction
                        {
                            AccountId = gremitApplication.AccountId,
                            Reference = transaction.ReferenceNo,
                            Gateway = PaymentGateways.Gremit,
                            Request = JsonConvert.SerializeObject(transaction),
                            Status = GRemitStatuses.Ready,
                            Response = string.Empty
                        });
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError("GRemitService", "LogDepositAsync", ex);
                    }
                }

                if (platformTransactions.Count > 0) insert = await _platformTransferRepository.BulkInsertAsync(platformTransactions);
                if (!insert)
                {
                    _logService.LogInfo("GRemitService", "LogDepositAsync", $"Unable to insert bulk references: {response.Result.Details.Transaction.Count} for {gremitApplication.Name} into gateway payout table");
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "LogDepositAsync", ex);
                return false;
            }

            return true;
        }

        public async Task ProcessAsync()
        {
            try
            {
                var platformTransactions = await _platformTransferRepository.GetByStatusAsync(GRemitStatuses.Ready);

                foreach (var platformTransaction in platformTransactions)
                {
                    try
                    {
                        string bankCode = "";

                        if (string.IsNullOrEmpty(platformTransaction.Request))
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Request is null:: Reference :{platformTransaction.Reference}");
                            continue;
                        }

                        var transaction = JsonConvert.DeserializeObject<GRemitTransaction>(platformTransaction.Request);
                        if (transaction is null)
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Unable to deserialize Request:: Reference :{platformTransaction.Reference}");
                            continue;
                        }

                        var transfer = await _transferRepository.DetailsAsync(transaction.ReferenceNo);
                        if (transfer is not null)
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Reference already exists on transfer:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        if (string.IsNullOrEmpty(transaction.Receiver.BeneficiaryBankCode))
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Empty Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}");

                            platformTransaction.Status = GRemitStatuses.Error;
                            platformTransaction.Response = $"Empty Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}";
                            await _platformTransferRepository.UpdateAsync(platformTransaction);

                            continue;
                        }

                        var gremitBank = StaticData.GRemitBanks.FirstOrDefault(x => x.Code == transaction.Receiver.BeneficiaryBankCode);
                        if (gremitBank is null && transaction.Receiver.BeneficiaryBankCode.Length == 3)
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Invalid Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}");

                            platformTransaction.Status = GRemitStatuses.Error;
                            platformTransaction.Response = $"Invalid Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}";
                            await _platformTransferRepository.UpdateAsync(platformTransaction);

                            continue;
                        }

                        bankCode = gremitBank == null && transaction.Receiver.BeneficiaryBankCode.Length > 3 ? transaction.Receiver.BeneficiaryBankCode : gremitBank.GenericCode;

                        var bank = StaticData.Banks.FirstOrDefault(x => x.Code == bankCode);
                        if (bank is null)
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Invalid Bank Code for GabTrans Core:: Reference :{transaction.ReferenceNo}");

                            platformTransaction.Status = GRemitStatuses.Error;
                            platformTransaction.Response = $"Invalid Bank Code for GabTrans Core:: Reference :{transaction.ReferenceNo}";
                            await _platformTransferRepository.UpdateAsync(platformTransaction);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "NGA") transaction.Receiver.CountryCode = "NG";

                        if (string.Equals(transaction.Remittance.ReceivingCurrency, Currencies.NGN, StringComparison.OrdinalIgnoreCase) && bank.Code == "000027") bank.Code = "103";

                        var lookUpResponse = await _globusBankService.GetNameEnquiryAsync(transaction.Receiver.AccountNo, bank.Code);
                        if (!lookUpResponse.Success)
                        {
                            _logService.LogInfo("GRemitService", "ProcessAsync", $"Unable to validate account for reference: {transaction.ReferenceNo}");

                            platformTransaction.Status = GRemitStatuses.Error;
                            platformTransaction.Response = $"Unable to validate account for reference: {transaction.ReferenceNo}";
                            await _platformTransferRepository.UpdateAsync(platformTransaction);
                            continue;
                        }

                        string payer = $"{transaction.Sender.FirstName} {transaction.Sender.LastName} {transaction.Sender.MiddleName}";

                        string payerAddress = $"{transaction.Sender.AddressLine1} {transaction.Sender.ZipCode}, {transaction.Sender.CityName}, {transaction.Sender.CountryCode}";

                        var meta = new TransferMeta { sender_address = payerAddress, sender_name = payer, sender_currency = transaction.Remittance.SendingCurrency, sender_country = transaction.Sender.CountryCode, sender_amount = transaction.Remittance.SendingAmount };

                        var transferRequest = new BankTransferRequest
                        {
                            Reference = transaction.ReferenceNo,
                            AccountNumber = transaction.Receiver.AccountNo,
                            Amount = decimal.Parse(transaction.Remittance.ReceivingAmount, CultureInfo.InvariantCulture),
                            Currency = transaction.Remittance.ReceivingCurrency,
                            Reason = payer.Trim(),
                            BankCode = bank.Code,
                            BankName = bank.Name,
                            MetaData = JsonConvert.SerializeObject(meta),
                            AccountName = lookUpResponse.Data.ToString(),
                            AccountType = "Personal",
                            CountryCode = Countries.Nigeria,
                            PaymentMethod = PaymentMethods.Local
                        };

                        _logService.LogInfo("GRemitService", $"ProcessAsync:: Payout Payload for reference::{transaction.ReferenceNo} ", JsonConvert.SerializeObject(transferRequest));

                        var transferResponse = await _bankTransferService.TransferAsync(transferRequest, platformTransaction.AccountId);
                        if (!transferResponse.Success)
                        {
                            platformTransaction.Status = GRemitStatuses.Error;
                            platformTransaction.Response = transferResponse.Message;
                            await _platformTransferRepository.UpdateAsync(platformTransaction);

                            _logService.LogInfo("GRemitService", "ProcessAsync:: Unable to process request for ", transaction.ReferenceNo);
                            continue;
                        }

                        platformTransaction.Status = GRemitStatuses.Paying;
                        platformTransaction.Response = transferResponse.Message;
                        await _platformTransferRepository.UpdateAsync(platformTransaction);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError("GRemitService", "ProcessAsync", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "ProcessAsync", ex);
            }
        }
    }
}
