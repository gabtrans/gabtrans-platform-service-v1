using Azure.Storage.Blobs.Models;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class CryptoTradeRepository(GabTransContext context, ILogService logService) : ICryptoTradeRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<CryptoTrade> DetailsAsync(long id)
        {
            return await _context.CryptoTrades.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<CryptoTrade>> GetAsync(long accountId)
        {
            return await _context.CryptoTrades.Where(x => x.AccountId == accountId).ToListAsync();
        }

        public async Task<bool> InsertAsync(CryptoTrade cryptoTrade)
        {
            try
            {
                _context.CryptoTrades.Add(cryptoTrade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(CryptoTradeRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(CryptoTrade cryptoTrade)
        {
            try
            {
                _context.CryptoTrades.Update(cryptoTrade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(CryptoTradeRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryTransaction.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(queryTransaction.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryTransaction.EndDate) ? DateTime.Now : Convert.ToDateTime(queryTransaction.EndDate);

            return await (from c in _context.CryptoTrades.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == c.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                          (queryTransaction.AccountId == 0 || queryTransaction.AccountId == null || a.Id == queryTransaction.AccountId) &&
                          (string.IsNullOrEmpty(queryTransaction.AccountName) || a.Name.Contains(queryTransaction.AccountName)) &&
                          (string.IsNullOrEmpty(queryTransaction.Email) || u.EmailAddress == queryTransaction.Email) &&
                          (string.IsNullOrEmpty(queryTransaction.Status) || u.EmailAddress == queryTransaction.Status)
                          && (string.IsNullOrEmpty(queryTransaction.Reference) || c.Reference == queryTransaction.Reference)
                            && c.CreatedAt >= fromDate && c.CreatedAt <= toDate
                          select new TransactionModel
                          {
                              Id = c.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                              Reference = c.Reference,
                              TransactionType = TransactionTypes.Crypto,
                              Amount = $"{c.ToAsset}{c.FromAmount.ToString()}",
                              Status = c.Status
                          }).OrderByDescending(c=>c.Id).ToListAsync();
        }

        public async Task<long> GetCryptoTradeAsync()
        {
            return await _context.CryptoTrades.Where(p => p.Status == TransactionStatuses.Successful).CountAsync();
        }

        public async Task<List<SummaryValue>> RevenuesAsync()
        {
            var trades = await _context.CryptoTrades
            .Where(s => s.CreatedAt.Year == DateTime.Now.Year)
            .GroupBy(s => new
            {
                s.CreatedAt.Month
            }).Select(g => new
            {
                Month = g.Key.Month,
                TotalFee = g.Sum(s => s.FromAmount)
            }).ToListAsync(); // Materialize the query

            return trades
                .Select(g => new SummaryValue
                {
                    Name = new DateTime(DateTime.Now.Year, g.Month, 1).ToString("MMM"),
                    Value = g.TotalFee.ToString("N2")
                })
                .OrderBy(r => r.Name)
                .ToList();
        }

        public async Task<List<Asset>> GetAssetsAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<List<Network>> GetNetworksAsync()
        {
            return await _context.Networks.ToListAsync();
        }
    }
}
