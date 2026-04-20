using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IVirtualAccountService
    {
        Task GenerateAsync();
        Task<VirtualAccount> DetailsByAccountAsync(long accountId);
       // Task<VirtualAccount> DetailsByPaymentAsync(long accountId,long paymentTypeId);
        Task<BankObject> GetAccountAsync(long accountId, string currencyCode);
       // Task<FundAccountObject> GetAccountsAsync(long accountId, string currencyCode);
        Task<bool> CreateAsync(long accountId, string bankName, string accountNumber, string accountName, string routingCode, long paymentTypeId, bool isPrimary);
    }
}
