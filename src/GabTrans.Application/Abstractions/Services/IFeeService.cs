using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IFeeService
    {
        Task<ApiResponse> CreateAsync(FeeRequest request);
        Task<ApiResponse> UpdateAsync(FeeRequest request, long id);
        Task<decimal> GetAsync(long accountId, string transactionType, string currency, decimal amount);
        Task<decimal> GetAsync(long accountId, string transactionType, string currency, string methodType, decimal amount);
    }
}
