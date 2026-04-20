using GabTrans.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Enums;

namespace GabTrans.Infrastructure.Repositories
{
    public class RecipientRepository(GabTransContext context, ILogService logService) : IRecipientRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(TransferRecipient transferRecipient)
        {
            using var tranContext = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.TransferRecipients.Add(transferRecipient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("RecipientRepository", "InsertAsync", ex);
            }
            return false;
        }

        public async Task<long> CreateAsync(TransferRecipient transferRecipient)
        {

            try
            {
                _context.TransferRecipients.Add(transferRecipient);
                await _context.SaveChangesAsync();
                return transferRecipient.Id;
            }
            catch (Exception ex)
            {
                _logService.LogError("RecipientRepository", "CreateAsync", ex);
            }
            return 0;
        }

        public async Task<bool> DeleteAsync(TransferRecipient transferRecipient)
        {
            try
            {
                _context.Entry(transferRecipient).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("RecipientRepository", "InsertAsync", ex);
            }

            return false;
        }

        public async Task<TransferRecipient> DetailsAsync(long id)
        {
            return await _context.TransferRecipients.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TransferRecipient>> GetAsync(long accountId)
        {
            return await _context.TransferRecipients.AsNoTracking().Where(a => a.AccountId == accountId).ToListAsync();
        }

        public async Task<TransferRecipient> GetAsync(long accountId, string accountNumber, string iban, string currency,string accountType)
        {
            return await _context.TransferRecipients.AsNoTracking().Where(a => a.AccountId == accountId && a.Currency==currency && a.BankAccountType==accountType && (a.AccountNumber==accountNumber || string.IsNullOrEmpty(accountNumber))).FirstOrDefaultAsync();
        }

        public async Task<List<TransferRecipient>> GetAsync(long? accountId, string type, string startDate, string endDate)
        {
            DateTime fromDate = string.IsNullOrEmpty(startDate) ? DateTime.Now.Date : Convert.ToDateTime(startDate);
            DateTime toDate = string.IsNullOrEmpty(endDate) ? DateTime.Now : Convert.ToDateTime(endDate);

            return await (from r in _context.TransferRecipients.AsNoTracking()
                          where (r.AccountId == accountId || accountId == 0 || accountId == null)
                    && (r.Type == type || string.IsNullOrEmpty(type))
                   && r.CreatedAt >= fromDate && r.CreatedAt <= toDate
                          select r
                              ).ToListAsync();
        }

        public async Task<TransferRecipient> GetAsync(long accountId, string accountNumber, string currency, string accountType)
        {
            return await _context.TransferRecipients.AsNoTracking().Where(a => a.AccountId == accountId && a.Currency == currency && a.BankAccountType == accountType && a.AccountNumber == accountNumber).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(TransferRecipient transferRecipient)
        {
            try
            {
                _context.TransferRecipients.Update(transferRecipient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("RecipientRepository", "UpdateAsync", ex);
            }

            return false;
        }
    }
}
