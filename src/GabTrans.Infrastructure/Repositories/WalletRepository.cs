using Microsoft.EntityFrameworkCore;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Logging;

namespace GabTrans.Infrastructure.Repositories
{
    public class WalletRepository(GabTransContext context, ILogService logService) : IWalletRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> CreateAsync(long accountId, string currency, string provider, string uuid)
        {
            try
            {
                var wallet = await _context.Wallets.Where(x => x.AccountId == accountId && x.Currency == currency).FirstOrDefaultAsync();
                if (wallet is not null) return true;

                _context.Add(new Wallet
                {
                    Currency = currency,
                    AccountId = accountId,
                    Uuid = uuid,

                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(WalletRepository), nameof(CreateAsync), ex);
            }

            return false;
        }

        public async Task<AccountWalletModel?> DetailsAsync(long id)
        {
            return await _context.Wallets.Where(x => x.Id == id).Select(u => new AccountWalletModel { AccountId = u.AccountId, CreatedOn = u.CreatedAt, CurrencyCode = u.Currency, CurrentBalance = u.Balance, Id = u.Id, UpdatedOn = u.UpdatedAt }).FirstOrDefaultAsync();
        }

        public async Task<Wallet> GetByCurrencyAsync(long accountId, string currencyCode)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.AccountId == accountId && x.Currency == currencyCode);
        }

        public async Task<List<Wallet>> GetAsync(long accountId)
        {
            return await _context.Wallets.Where(x => x.AccountId == accountId).ToListAsync();
        }

        public async Task<bool> UpdateAsync(long accountId, decimal limitAmount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (wallet is null) return false;
            //wallet.WalletLimit=limitAmount;
            wallet.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetCurrentBalanceAsync(long accountId, string currency)
        {
            var balance = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountId == accountId && x.Currency == currency);
            return balance == null ? 0 : balance.Balance;
        }

        public async Task<Wallet> GetAsync(long accountId, string currency)
        {
            return await _context.Wallets.Where(x => x.AccountId == accountId && x.Currency == currency).FirstOrDefaultAsync();
        }

        public async Task<List<BalanceObject>> TotalBalancesAsync(long accountId)
        {
            return await _context.Wallets.Where(x => x.AccountId == accountId).Select(a => new BalanceObject { Amount = a.Balance, CurrencyCode = a.Currency }).ToListAsync();
        }
        public async Task<AccountWalletObject> GetBalanceAsync(long accountId)
        {
            return await (from w in _context.Wallets.AsNoTracking()
                          from cc in _context.Currencies.AsNoTracking().Where(cc => cc.Code == w.Currency).DefaultIfEmpty()
                          from cs in _context.Countries.AsNoTracking().Where(cs => cs.Code == cc.CountryCode).DefaultIfEmpty()
                          where w.AccountId == accountId
                          select new AccountWalletObject
                          {
                              Balance = w.Balance.ToString("N2"),
                              Code = cc.Code,
                              //Flag = cs.Flag,
                              //Name = cs.CurrencyName,
                              Symbol = cc.Symbol,
                              Id = w.Id
                          }).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(Wallet wallet)
        {
            try
            {
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(WalletRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Wallet wallet)
        {
            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(WalletRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
