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
                var applicationIds = StaticData.GremitApplications.Where(x => string.Equals(x.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase)).DistinctBy(x => x.AccountId).Select(x => x.AccountId).ToList();

                var transfers = await _platformTransferRepository.GetAsync(GRemitStatuses.Paying, applicationIds);

                if (transfers.Count() > 0) _logService.LogInfo("GRemitService", "ConfirmationAsync", $"Total number of payouts is : {transfers.Count()}");

                foreach (var transfer in transfers)
                {
                    var details = await _transferRepository.DetailsAsync(transfer.Reference);
                    if (details is null) continue;

                    if (string.Equals(details.Status, TransactionStatuses.Pending, StringComparison.OrdinalIgnoreCase)) continue;

                    string reason = details.FailureReason ?? "session timeout from the beneficiary bank";

                    var gremitApplication = StaticData.GremitApplications.Where(x => x.AccountId == transfer.AccountId && x.Country == Countries.Nigeria).FirstOrDefault();
                    if (gremitApplication is null) continue;

                    switch (details.Status)
                    {
                        case TransactionStatuses.Success:
                            await ApprovedAsync(gremitApplication, transfer.Reference);
                            break;
                        case TransactionStatuses.Reversed:
                            await RejectAsync(gremitApplication, transfer.Reference, reason);
                            break;
                        case TransactionStatuses.Failed:
                            await RejectAsync(gremitApplication, transfer.Reference, reason);
                            break;
                        default:
                            break;
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
            string updatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var approve = await _gRemitClientIntegration.ApproveAsync(gremitApplication, reference, updatedAt);
            if (approve is null || approve.Result is null) return;

            if (approve.Result.ResultCode != "1000") return;

            await _platformTransferRepository.UpdateStatusAsync(reference, GRemitStatuses.Paid, JsonConvert.SerializeObject(approve));
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
                    string bankCode = "";

                    var payoutDetails = await _platformTransferRepository.DetailsAsync(transaction.ReferenceNo);
                    if (payoutDetails is not null && !string.IsNullOrEmpty(payoutDetails.Response)) continue;

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
                    if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "GHA") transaction.Receiver.CountryCode = "GH";
                    if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "KEN") transaction.Receiver.CountryCode = "KE";
                    if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "ZAF") transaction.Receiver.CountryCode = "ZA";

                    var lookUpResponse = await _globusBankService.GetNameEnquiryAsync(transaction.Receiver.AccountNo, transaction.Receiver.BeneficiaryBankCode);
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
                        await _platformTransferRepository.UpdateAsync(transaction.ReferenceNo, GRemitStatuses.Error);

                        _logService.LogInfo("GRemitService", "DepositAsync:: Unable to process request for ", transaction.ReferenceNo);
                        continue;
                    }

                    await _platformTransferRepository.UpdateAsync(transaction.ReferenceNo, GRemitStatuses.Paying);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitService", "DepositAsync", ex);
                return false;
            }

            return true;
        }

        //public async Task<bool> MobileMoneyAsync(GremitApplication gremitApplication)
        //{
        //    try
        //    {
        //        var response = await _gRemitClientIntegration.GetTransactionsAsync(gremitApplication);
        //        if (response is null || response.Result is null) return false;

        //        if (!response.Result.ResultCode.Equals("1000")) return false;

        //        if (response.Result.Details.Transaction.Count > 0) _logService.LogInfo("GRemitService", "MobileMoneyAsync", $"Total number of transactions retrieved from GRemit is : {response.Result.Details.Transaction.Count}");

        //        foreach (var transaction in response.Result.Details.Transaction)
        //        {
        //            string bankCode = "";

        //            var payoutDetails = await _gatewayPayoutRepository.DetailsAsync(transaction.ReferenceNo);
        //            if (payoutDetails is not null) continue;

        //            var mobileOperator = StaticData.MobileMoneyProviders.FirstOrDefault(x => x.Name == transaction.Receiver.BeneficiaryBankCode);
        //            if (mobileOperator is null)
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync", $"Invalid Bank Code for GRemit:: Reference :{transaction.ReferenceNo}");
        //                continue;
        //            }

        //            if (mobileOperator is not null) bankCode = mobileOperator.Name;

        //            if (string.IsNullOrEmpty(transaction.Receiver.BeneficiaryBankCode))
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync", $"Bank Code is empty for GRemit:: Reference :{transaction.ReferenceNo}");
        //                continue;
        //            }

        //            if (transaction.Receiver.BeneficiaryBankCode.Length > 3) bankCode = transaction.Receiver.BeneficiaryBankCode;

        //            if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "NGA") transaction.Receiver.CountryCode = "NG";
        //            if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "GHA") transaction.Receiver.CountryCode = "GH";
        //            if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "KEN") transaction.Receiver.CountryCode = "KE";
        //            if (!string.IsNullOrEmpty(transaction.Receiver.CountryCode) && transaction.Receiver.CountryCode == "ZAF") transaction.Receiver.CountryCode = "ZA";

        //            //Get Fees
        //            var feeDetails = await _gatewayPayoutRepository.GetFeeAsync(gremitApplication.BusinessId, transaction.Remittance.ReceivingCurrency);
        //            if (feeDetails is null) continue;

        //            decimal fee = Convert.ToDecimal(feeDetails.Fee);

        //            decimal amount = Convert.ToDecimal(transaction.Remittance.ReceivingAmount);

        //            decimal totalAmount = Math.Round(amount + fee, 2);

        //            decimal balance = await walletTransactionService.GetBalanceAsync(gremitApplication.BusinessId, transaction.Remittance.ReceivingCurrency);
        //            if (balance <= totalAmount)
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync ", $"Low balance and unable to process payment for reference: {transaction.ReferenceNo}");
        //                continue;
        //            }

        //            var credentials = StaticData.BusinessApiCredentials.Where(x => x.BusinessId == gremitApplication.BusinessId).FirstOrDefault();
        //            if (credentials is null)
        //            {
        //                _logService.LogInfo("GRemitService", $"MobileMoneyAsync:: Unable to fetch credentials for reference::", $"{transaction.ReferenceNo}");
        //                continue;
        //            }

        //            string publicKey = credentials.TestPubKey;
        //            string secretKey = credentials.TestSecKey;

        //            if (StaticData.Domain == Domains.Live)
        //            {
        //                publicKey = credentials.LivePubKey;
        //                secretKey = credentials.LiveSecKey;
        //            }

        //            var account = await _accountService.ValidateAsync(transaction.Receiver.AccountNo, bankCode, secretKey);
        //            if (string.IsNullOrEmpty(account))
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync", $"Unable to validate account for reference: {transaction.ReferenceNo}");
        //                continue;
        //            }

        //            bool insert = await _gatewayPayoutRepository.InsertAsync(gremitApplication.BusinessId, transaction.ReferenceNo, JsonConvert.SerializeObject(transaction), Gateways.GRemit);
        //            if (!insert)
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync", $"Unable to insert reference: {transaction.ReferenceNo} into gateway payout table");
        //                continue;
        //            }

        //            string payer = string.Concat(transaction.Sender.FirstName, " ", transaction.Sender.LastName, " ", transaction.Sender.MiddleName);

        //            string payerAddress = string.Concat(transaction.Sender.AddressLine1, " ", transaction.Sender.CityName, " ", transaction.Sender.StateCode, ", ", transaction.Sender.CountryCode);

        //            var meta = new PayoutMeta { sender_address = payerAddress, sender_name = payer };

        //            var payoutRequest = new BudPayoutRequest
        //            {
        //                reference = transaction.ReferenceNo,
        //                account_number = transaction.Receiver.AccountNo,
        //                amount = transaction.Remittance.ReceivingAmount,
        //                currency = transaction.Remittance.ReceivingCurrency,
        //                narration = payer.Trim(),
        //                bank_code = bankCode,//loop valid 
        //                bank_name = transaction.Receiver.BeneficiaryBankName
        //            };

        //            _logService.LogInfo("GRemitService", $"MobileMoneyAsync:: Payout Payload for reference::{transaction.ReferenceNo} ", JsonConvert.SerializeObject(payoutRequest));

        //            //Create Payout
        //            var result = await merchantService.InitiatePayoutAsync(payoutRequest, publicKey, secretKey);
        //            if (!result.Status)
        //            {
        //                _logService.LogInfo("GRemitService", "MobileMoneyAsync:: Unable to process request for ", transaction.ReferenceNo);
        //                continue;
        //            }

        //            await _gatewayPayoutRepository.UpdateStatusAsync(transaction.ReferenceNo, RemitlyStatuses.Paying);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("GRemitService", "MobileMoneyAsync", ex);
        //        return false;
        //    }

        //    return true;
        //}

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
    }
}
