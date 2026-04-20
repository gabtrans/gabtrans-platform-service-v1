using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IDepositService
    {
        Task ProcessAsync();
        Task<ApiResponse> GetTransactionsAsync(GetTransactionRequest request);
        Task<FeeObject> GetFeeAsync(long transferId, decimal amount, string countryCode);
        Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction);
    }
}
