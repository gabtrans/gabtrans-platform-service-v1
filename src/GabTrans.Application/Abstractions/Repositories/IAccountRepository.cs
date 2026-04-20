using System;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> DetailsAsync(long id);
        Task<Account> DetailsByUuidAsync(string id);
        Task<bool> DoesNameExistAsync(string name);
        Task<AccountType> AccountTypeAsync(string name);
        Task<List<IdNameObject>> AccountTypesAsync();
        Task<AccountType> AccountTypeAsync(long id);
        Task<List<Account>> GetExistingAccountsAsync(long userId);
        Task<List<Account>> GetAccountsAsync(long userId);
        Task<Account> GetAccountAsync(long userId);
        Task<List<Account>> GetAsync(string status);
        Task<List<Account>> WithoutVirtualAsync();
        Task<bool> UpdateAsync(Account account);
        Task<long> InsertAsync(Account account);
        Task<bool> BulkUpdateAsync(List<Account> accounts);
        Task<List<Account>> GetExistingAccountsAsync(long userId, string accountType);
        Task<AccountDetailsModel> GetDetailByUserAsync(long userId);
        Task<AccountDetailsModel> GetAccountDetailsAsync(long accountId);
        Task<List<AccountBalanceModel>> GetAccountBalancesAsync(long userId);
    }
}

