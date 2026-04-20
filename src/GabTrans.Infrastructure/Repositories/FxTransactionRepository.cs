using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class FxTransactionRepository(GabTransContext context, ILogService logService) : IFxTransactionRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<List<FxTransaction>> GetAsync(long accountId)
        {
            return await _context.FxTransactions.Where(x => x.AccountId == accountId).ToListAsync();
        }

        public async Task<bool> InsertAsync(FxTransaction fxTransaction)
        {
            try
            {
                _context.FxTransactions.Add(fxTransaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxTransactionRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(FxTransaction fxTransaction)
        {
            try
            {
                _context.FxTransactions.Update(fxTransaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxTransactionRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryTransaction.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(queryTransaction.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryTransaction.EndDate) ? DateTime.Now : Convert.ToDateTime(queryTransaction.EndDate);

            return await (from fx in _context.FxTransactions.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == fx.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                          (queryTransaction.AccountId == 0 || queryTransaction.AccountId == null || a.Id == queryTransaction.AccountId) &&
                          (string.IsNullOrEmpty(queryTransaction.AccountName) || a.Name.Contains(queryTransaction.AccountName)) &&
                          (string.IsNullOrEmpty(queryTransaction.Email) || u.EmailAddress == queryTransaction.Email) &&
                          (string.IsNullOrEmpty(queryTransaction.Status) || u.EmailAddress == queryTransaction.Status) &&
                          (string.IsNullOrEmpty(queryTransaction.Reference) || fx.Reference == queryTransaction.Reference)

                            && fx.CreatedAt >= fromDate && fx.CreatedAt <= toDate
                          select new TransactionModel
                          {
                              Id = fx.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = fx.CreatedAt.ToString("MMMM-dd, yyyy HH:mm:ss"),
                              Reference = fx.Reference,
                              TransactionType = TransactionTypes.Conversion,
                              Amount = $"{fx.ToCurrency}{fx.FromAmount.ToString("N2")}",
                              Status = fx.Status
                          }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<FxTransactionModel> DetailsAsync(long id)
        {
            return await (from fx in _context.FxTransactions.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == fx.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                            fx.Id == id
                          select new FxTransactionModel
                          {
                              Id = fx.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = fx.CreatedAt.ToString("MMMM-dd, yyyy HH:mm:ss"),
                              TransactionReference = fx.Reference,
                              TransactionType = TransactionTypes.Conversion,
                              Amount = $"{fx.FromCurrency}{fx.FromAmount.ToString("N2")}",
                              Status = fx.Status,
                              AccountType = a.Type,
                              ConvertedAmount = $"{fx.ToCurrency}{fx.FromAmount.ToString("N2")}",
                              FromCurrency = fx.FromCurrency,
                              ToCurrency = fx.ToCurrency,
                              TransactionFee = "0"
                          }).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<long> GetCurrencyConversionAsync()
        {
            return await _context.FxTransactions.Where(p => p.Status == TransactionStatuses.Successful).CountAsync();
        }

        public async Task<List<SummaryValue>> RevenuesAsync()
        {
            var transactions = await _context.FxTransactions
            .Where(s => s.CreatedAt.Year == DateTime.Now.Year)
            .GroupBy(s => new
            {
                s.CreatedAt.Month
            }).Select(g => new
            {
                Month = g.Key.Month,
                TotalFee = g.Sum(s => s.FromAmount)
            }).ToListAsync(); // Materialize the query

            return transactions
                .Select(g => new SummaryValue
                {
                    Name = new DateTime(DateTime.Now.Year, g.Month, 1).ToString("MMM"),
                    Value = g.TotalFee.ToString("N2")
                })
                .OrderBy(r => r.Name)
                .ToList();
        }
    }
}
