

using GabTrans.Application.DataTransfer;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IWalletTransactionService
    {
        //Task<string?> UpdateAsync(string referenceNumber, long status);
        //Task<string?> ProcessTransactionAsync(long accountId, string referenceNumber, string currency, decimal amount, decimal fee, long transactionTypeId, bool isDebit = true);
        Task<ApiResponse> DebitAsync(long accountId, string referenceNumber, string currency, decimal amount, decimal fee, long transactionTypeId,string category);
        Task<ApiResponse> CreditAsync(long accountId, string referenceNumber, string currency, decimal amount, decimal fee, long transactionTypeId,string category);
    }
}
