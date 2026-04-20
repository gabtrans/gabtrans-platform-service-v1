using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GabTrans.Infrastructure.Repositories
{
    public class LimitRepository(GabTransContext context, ILogService logService) : ILimitRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<LimitModel> DetailsAsync(long id)
        {
            return await (from l in _context.Limits.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == l.AccountId).DefaultIfEmpty()
                          where l.Id==id
                          
                          select new LimitModel
                          {
                              AccountId = l.AccountId,
                              Id = l.Id,
                              CreatedAt = l.CreatedAt,
                              Currency = l.Currency,
                              AccountType = l.AccountType,
                              DailyCount = l.DailyCount,
                              DailyCumulative = l.DailyCumulative,
                              SingleCumulative = l.SingleCumulative,
                              TransactionType = l.TransactionType,
                              UpdatedAt = l.UpdatedAt,
                              AccountName = a != null ? a.Name : string.Empty
                          }).FirstOrDefaultAsync();
        }

        public async Task<Limit> DetailsByIdAsync(long id)
        {
            return await _context.Limits.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Limit> DetailsByAccountTypeAsync(long accountTypeId)
        {
            return await _context.Limits.FirstOrDefaultAsync(x => x.Id == accountTypeId);
        }

        public async Task<Limit> GetAsync(long accountId, string type)
        {
            var limit = await _context.Limits.AsNoTracking().Where(x => x.AccountId == accountId && x.TransactionType == type).FirstOrDefaultAsync();
            if (limit is not null) return limit;

            return await _context.Limits.AsNoTracking().Where(x => x.AccountId == 0 && x.TransactionType == type).FirstOrDefaultAsync();
        }

        public async Task<Limit> GetAsync(long accountId, string type, string currency)
        {
            var limit = await _context.Limits.AsNoTracking().Where(x => x.AccountId == accountId && x.TransactionType == type && x.Currency == currency).FirstOrDefaultAsync();
            if (limit is not null) return limit;

            return await _context.Limits.AsNoTracking().Where(x => x.AccountId == 0 && x.TransactionType == type && x.Currency == currency).FirstOrDefaultAsync();
        }

        public async Task<List<LimitModel>> GetAsync(QueryLimit queryLimit)
        {
            //DateTime fromDate = string.IsNullOrEmpty(queryLimit.StartDate) ? DateTime.Now.AddDays(-) : Convert.ToDateTime(queryLimit.StartDate);
            //DateTime toDate = string.IsNullOrEmpty(queryLimit.EndDate) ? DateTime.Now : Convert.ToDateTime(queryLimit.EndDate);

            return await (from l in _context.Limits.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == l.AccountId).DefaultIfEmpty()
                          where
                          (queryLimit.AccountId == 0 || queryLimit.AccountId == null || l.AccountId == queryLimit.AccountId) &&
                          (string.IsNullOrEmpty(queryLimit.TransactionType) || l.TransactionType == queryLimit.TransactionType)
                         &&  (string.IsNullOrEmpty(queryLimit.Currency) || l.Currency == queryLimit.Currency)
                          // && l.CreatedAt.Date >= fromDate.Date && l.CreatedAt.Date <= toDate.Date
                          select new LimitModel
                          {
                              AccountId = l.AccountId,
                              Id = l.Id,
                              CreatedAt = l.CreatedAt,
                              Currency = l.Currency,
                              AccountType = l.AccountType,
                              DailyCount = l.DailyCount,
                              DailyCumulative = l.DailyCumulative,
                              SingleCumulative = l.SingleCumulative,
                              TransactionType = l.TransactionType,
                              UpdatedAt = l.UpdatedAt,
                              AccountName = a != null ? a.Name : string.Empty
                          }).ToListAsync();
        }

        public async Task<bool> InsertAsync(Limit limit)
        {
            try
            {
                _context.Limits.Add(limit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LimitRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Limit limit)
        {
            try
            {
                _context.Limits.Update(limit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LimitRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<Limit> GetByCurrencyAsync(string currency, string type)
        {
            return await _context.Limits.Where(x => x.Currency == currency && x.TransactionType == type).FirstOrDefaultAsync();
        }

        public async Task<Limit> GetByCurrencyAsync(string currency, string type, long accountId)
        {
            return await _context.Limits.Where(x => x.Currency == currency && x.TransactionType == type && x.AccountId == accountId).FirstOrDefaultAsync();
        }
    }
}

