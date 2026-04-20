using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories
{
    public class FeeRepository(GabTransContext context, ILogService logService) : IFeeRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(Fee fee)
        {
            try
            {
                _context.Fees.Add(fee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FeeRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<Fee> GetAsync(long accountId, string transactionType, string currency)
        {
            var fee = await _context.Fees.Where(x => x.TransactionType == transactionType && x.Currency == currency && x.AccountId == accountId).FirstOrDefaultAsync();
            if (fee is not null) return fee;

            return await _context.Fees.Where(x => x.TransactionType == transactionType && x.Currency == currency && x.AccountId == 0).FirstOrDefaultAsync();
        }

        public async Task<Fee> GetAsync(long accountId, string transactionType, string currency, string methodType)
        {
            var fee = await _context.Fees.Where(x => x.TransactionType == transactionType && x.Currency == currency && x.MethodType == methodType && x.AccountId == accountId).FirstOrDefaultAsync();
            if (fee is not null) return fee;

            return await _context.Fees.Where(x => x.TransactionType == transactionType && x.Currency == currency && x.MethodType == methodType && x.AccountId == 0).FirstOrDefaultAsync();
        }

        public async Task<Fee> DetailsAsync(long id)
        {
            return await _context.Fees.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Fee> GetByCurrencyAsync(string currency, string type)
        {
            return await _context.Fees.Where(x => x.Currency == currency && x.TransactionType == type).FirstOrDefaultAsync();
        }

        public async Task<Fee> GetByCurrencyAsync(string currency, string type, long accountId)
        {
            return await _context.Fees.Where(x => x.Currency == currency && x.TransactionType == type && x.AccountId == accountId).FirstOrDefaultAsync();
        }

        public async Task<List<Fee>> GetAsync()
        {
            return await _context.Fees.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Fee fee)
        {
            try
            {
                _context.Fees.Update(fee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FeeRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<List<FeeModel>> GetAsync(QueryFee queryFee)
        {
            //DateTime fromDate = string.IsNullOrEmpty(queryFee.StartDate) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(queryFee.StartDate);
            //DateTime toDate = string.IsNullOrEmpty(queryFee.EndDate) ? DateTime.Now : Convert.ToDateTime(queryFee.EndDate);

            return await (from f in _context.Fees.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == f.AccountId).DefaultIfEmpty()
                          where
                          (queryFee.AccountId == 0 || queryFee.AccountId == null || f.AccountId == queryFee.AccountId) &&
                          (string.IsNullOrEmpty(queryFee.Currency) || f.Currency == queryFee.Currency)
                          // && f.CreatedAt.Date >= fromDate.Date && f.CreatedAt.Date <= toDate.Date
                          select new FeeModel
                          {
                              AccountId = f.AccountId,
                              Id = f.Id,
                              CreatedAt = f.CreatedAt,
                              Currency = f.Currency,
                              MaxAmount = f.CappedValue,
                              TransactionType = f.TransactionType,
                              Rate = f.Rate,
                              UpdatedAt = f.UpdatedAt,
                              AccountName = a != null ? a.Name : string.Empty
                          }).ToListAsync();
        }
    }
}
