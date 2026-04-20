using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IWalletRepository
    {
        Task<bool> InsertAsync(Wallet wallet);
        Task<bool> UpdateAsync(Wallet wallet);
        Task<AccountWalletModel?> DetailsAsync(long Id);
        Task<List<BalanceObject>> TotalBalancesAsync(long accountId);
        Task<Wallet> GetByCurrencyAsync(long accountId, string currency);
        Task<List<Wallet>> GetAsync(long accountId);
        Task<Wallet> GetAsync(long accountId, string currency);
        Task<bool> UpdateAsync(long accountId, decimal limitAmount);
        Task<decimal> GetCurrentBalanceAsync(long accountId, string currency);
        Task<AccountWalletObject> GetBalanceAsync(long accountId);
        Task<bool> CreateAsync(long accountId, string currency, string provider, string uuid);
    }
}
