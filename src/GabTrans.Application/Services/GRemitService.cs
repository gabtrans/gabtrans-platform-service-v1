using Newtonsoft.Json;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System.Net;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Interfaces.Services;
using System.Globalization;

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

        public async Task ConfirmationAsync()
        {
            try
            {
                var applicationIds = StaticData.GremitAccounts.Where(x => string.Equals(x.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase)).DistinctBy(x => x.AccountId).Select(x => x.AccountId).ToList();

                var transfers = await _platformTransferRepository.GetAsync(GRemitStatuses.Paying, applicationIds);

                if (transfers.Count() > 0) _logService.LogInfo("GRemitService", "ConfirmationAsync", $"Total number of payouts is : {transfers.Count()}");

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

                        //switch (details.Status)
                        //{
                        //    case TransactionStatuses.Successful:
                        //        await ApprovedAsync(gremitApplication, transfer.Reference);
                        //        break;
                        //    case TransactionStatuses.Reversed:
                        //        await RejectAsync(gremitApplication, transfer.Reference, reason);
                        //        break;
                        //    case TransactionStatuses.Failed:
                        //        await RejectAsync(gremitApplication, transfer.Reference, reason);
                        //        break;
                        //    default:
                        //        break;
                        //}

                        if (string.Equals(details.Status, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase)) await ApprovedAsync(gremitApplication, transfer.Reference);

                        if (string.Equals(details.Status, TransactionStatuses.Reversed, StringComparison.OrdinalIgnoreCase)) await RejectAsync(gremitApplication, transfer.Reference, reason);

                        if (string.Equals(details.Status, TransactionStatuses.Failed, StringComparison.OrdinalIgnoreCase)) await RejectAsync(gremitApplication, transfer.Reference, reason);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError("GRemitService", "ConfirmationAsync", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "ConfirmationAsync", ex);
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

        public async Task<bool> DepositAsync(GremitAccount gremitApplication)
        {
            try
            {
                var response = await _gRemitClientIntegration.GetTransactionsAsync(gremitApplication);
                if (response is null || response.Result is null) return false;

                if (!response.Result.ResultCode.Equals("1000")) return false;

                if (response.Result.Details.Transaction.Count > 0) _logService.LogInfo("GRemitService", "DepositAsync", $"Total number of transactions retrieved from GRemit is : {response.Result.Details.Transaction.Count} for {gremitApplication.Name}");

                foreach (var transaction in response.Result.Details.Transaction)
                {
                    try
                    {
                        string bankCode = "";

                        var platformDetails = await _platformTransferRepository.DetailsAsync(transaction.ReferenceNo);
                        if (platformDetails is not null && !string.Equals(platformDetails.Status, GRemitStatuses.Ready, StringComparison.OrdinalIgnoreCase))
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Reference already exists on platform:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        var transfer = await _transferRepository.DetailsAsync(transaction.ReferenceNo);
                        if (transfer is not null)
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Reference already exists on transfer:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        if (string.IsNullOrEmpty(transaction.Receiver.BeneficiaryBankCode))
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Empty Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        var gremitBank = StaticData.GRemitBanks.FirstOrDefault(x => x.Code == transaction.Receiver.BeneficiaryBankCode);
                        if (gremitBank is null && transaction.Receiver.BeneficiaryBankCode.Length == 3)
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Invalid Bank Code for GRemit Banks:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        bankCode = gremitBank == null && transaction.Receiver.BeneficiaryBankCode.Length > 3 ? transaction.Receiver.BeneficiaryBankCode : gremitBank.GenericCode;

                        _logService.LogInfo("GRemitService", "DepositAsync", $"Bank Code for BudPay:: Reference :{transaction.ReferenceNo} is {bankCode}");

                        var bank = StaticData.Banks.FirstOrDefault(x => x.Code == bankCode);
                        if (bank is null)
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Invalid Bank Code for BudPay:: Reference :{transaction.ReferenceNo}");
                            continue;
                        }

                        if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "NGA") transaction.Receiver.CountryCode = "NG";

                        var lookUpResponse = await _globusBankService.GetNameEnquiryAsync(transaction.Receiver.AccountNo, bank.Code);
                        if (!lookUpResponse.Success)
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Unable to validate account for reference: {transaction.ReferenceNo}");
                            continue;
                        }

                        bool insert = await _platformTransferRepository.InsertAsync(gremitApplication.AccountId, transaction.ReferenceNo, JsonConvert.SerializeObject(transaction), PaymentGateways.Gremit);
                        if (!insert)
                        {
                            _logService.LogInfo("GRemitService", "DepositAsync", $"Unable to insert reference: {transaction.ReferenceNo} into gateway payout table");
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

                        _logService.LogInfo("GRemitService", $"DepositAsync:: Payout Payload for reference::{transaction.ReferenceNo} ", JsonConvert.SerializeObject(transferRequest));

                        var transferResponse = await _bankTransferService.TransferAsync(transferRequest, gremitApplication.AccountId);
                        if (!transferResponse.Success)
                        {
                            await _platformTransferRepository.UpdateStatusAsync(transaction.ReferenceNo, GRemitStatuses.Error, transferResponse.Message);

                            _logService.LogInfo("GRemitService", "DepositAsync:: Unable to process request for ", transaction.ReferenceNo);
                            continue;
                        }

                        await _platformTransferRepository.UpdateStatusAsync(transaction.ReferenceNo, GRemitStatuses.Paying, transferResponse.Message);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError("GRemitService", "DepositAsync", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "DepositAsync", ex);
                return false;
            }

            return true;
        }

        public async Task ProcessAsync()
        {
            try
            {
                int counter = 0;

                var applications = await _platformTransferRepository.GetGRemitAsync(AccountStatuses.Active);
                foreach (var application in applications)
                {
                    bool processed = false;

                    if (string.Equals(application.DeliveryMethod, GRemitDeliveryMethods.Deposit, StringComparison.OrdinalIgnoreCase)) processed = await DepositAsync(application);
                    if (processed) counter++;
                }

                if (counter > 0) _logService.LogInfo("GRemitService", "ProcessAsync", $"Processed {counter} applications");
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "ProcessAsync", ex);
            }
        }

        public async Task ReProcessAsync()
        {
            try
            {
                int counter = 0;

                var applications = await _platformTransferRepository.GetGRemitAsync(AccountStatuses.Active);
                foreach (var application in applications)
                {
                    bool processed = false;

                    if (string.Equals(application.DeliveryMethod, GRemitDeliveryMethods.Deposit, StringComparison.OrdinalIgnoreCase)) processed = await DepositAsync(application);
                    if (processed) counter++;
                }

                if (counter > 0) _logService.LogInfo("GRemitService", "ProcessAsync", $"Processed {counter} applications");
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "ProcessAsync", ex);
            }
        }
    }
}
