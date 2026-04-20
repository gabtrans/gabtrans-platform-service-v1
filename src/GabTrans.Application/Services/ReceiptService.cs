using GabTrans.Domain.Constants;
using GabTrans.Domain.Models;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;


namespace GabTrans.Application.Services
{
    public class ReceiptService(ILogService logService, IFileService fileService, ITransferRepository transferRepository, ISettlementRepository settlementRepository) : IReceiptService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly ITransferRepository _transferRepository = transferRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;

        public byte[]? GenerateReceipt(Receipt receipt)
        {
            string htmlContent = receipt.Template.Replace("{Provider}", receipt.Provider).Replace("{Product}", receipt.Bill ?? "N/A").Replace("{Currency}", receipt.Currency).Replace("{Amount}", receipt.Amount.ToString("N2")).Replace("{TransactionDate}", receipt.DateCreated.ToString("dd MMM, yyyy")).Replace("{ReferenceNumber}", receipt.ReferenceNumber).Replace("{Number}", receipt.ReceiverAccountNumber ?? "N/A").Replace("{AccountName}", receipt.ReceiverAccountName).Replace("{Bank}", receipt.ReceiverBank).Replace("{PaymentType}", receipt.PaymentType).Replace("{SenderName}", receipt.SenderAccountName ?? "N/A").Replace("{Package}", receipt.Package ?? "N/A").Replace("{BeneficiaryName}", receipt.ReceiverAccountName ?? "N/A").Replace("{Status}", receipt.TransactionStatus).Replace("{SenderAccount}", receipt.SenderAccountNumber ?? "N/A").Replace("{Narration}", receipt.Narration ?? "N/A").Replace("{BeneficiaryAccount}", receipt.ReceiverAccountNumber ?? "N/A").Replace("{AccountType}", receipt.AccountType).Replace("{CreditToken}", receipt.CreditToken ?? "N/A").Replace("{Units}", receipt.Units ?? "N/A");

            return _fileService.ConvertToPdf(htmlContent);
        }

        public byte[]? GetTemplate(Receipt receipt)
        {
            string template = string.Empty;

            //switch (receipt.TransactionTypeId)
            //{
            //    case TransactionTypes.SendMoney:
            //        template = Domain.Constants.Receipts.SendMoney;
            //        //if (receipt.Status == Status.Declined) template = Domain.Constants.Receipts.FailedSendMoney;
            //        break;
            //    case TransactionTypes.BillPayment:
            //        template = Domain.Constants.Receipts.Electricity;
            //        if (receipt.BillId == Bills.Tv_Subscription) template = Domain.Constants.Receipts.Cable;
            //        break;
            //    default:
            //        break;
            //}

            receipt.Template = _fileService.GetTemplate(template, Templates.Receipt);
            return GenerateReceipt(receipt);
        }

        public async Task<ApiResponse> GenerateAsync(string referenceNumber)
        {
            var result = new ApiResponse();
            try
            {
                // var details = await _walletRepository.DetailsAsync(referenceNumber);
                // if (details is null)
                // {
                //     return new AppResult
                //     {
                //         Message = "Details not found"
                //     };
                // }


                // var receipt = GetTemplate(details);
                // if (receipt is null)
                // {
                //     return new AppResult
                //     {
                        
                //         Message = "Unable to get template"
                //     };
                // }

                result.Success = true;
                result.Message = "Success";
                //result.Data = receipt;
            }
            catch (Exception ex)
            {
                _logService.LogError("ReceiptService", "GenerateAsync", ex);
                
                result.Message = "Kindly try again later";
            }
            return result;
        }

    public async Task<string> TransferAsync(Settlement  settlement)
    {
        var details=await _transferRepository.DetailsAsync(settlement.Reference);
        if(details is null) return string.Empty;

        return "";
    }

    }
}
