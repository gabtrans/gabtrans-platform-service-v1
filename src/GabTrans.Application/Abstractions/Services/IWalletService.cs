using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IWalletService
    {
        Task<bool> CreateAsync(long accountId, string currencyCode);
        Task<List<BalanceObject>> TotalBalancesAsync(long accountId);
        Task<decimal> GetCurrentBalanceAsync(long accountId, string currency);
    }
}
