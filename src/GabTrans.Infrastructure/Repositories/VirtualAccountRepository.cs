using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Twilio.Rest.Api.V2010.Account.Sip.Domain.AuthTypes.AuthTypeRegistrations;

namespace GabTrans.Infrastructure.Repositories
{
    public class VirtualAccountRepository(GabTransContext context, ILogService logService) : IVirtualAccountRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(VirtualAccount virtualAccount)
        {
            try
            {
                _context.VirtualAccounts.Add(virtualAccount);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(VirtualAccountRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(VirtualAccount virtualAccount)
        {
            try
            {
                _context.Update(virtualAccount);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(VirtualAccountRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<VirtualAccount> DetailsAsync(long id)
        {
            return await _context.VirtualAccounts.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccount> GetAsync(long accountId)
        {
            return await _context.VirtualAccounts.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccountModel> GetByUserIdAsync(long userId)
        {
            return await (from v in _context.VirtualAccounts.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == v.AccountId).DefaultIfEmpty()
                          from c in _context.Countries.AsNoTracking().Where(c => c.Code == v.Country).DefaultIfEmpty()
                          where a.UserId == userId
                          select new VirtualAccountModel
                          {
                              AccountHolderName = v.AccountHolderName,
                              AccountName = a.Name,
                              AccountNumber = v.AccountNumber,
                              AccountType = v.Type,
                              BankName = v.BankName,
                              BankCity = v.BankCity,
                              BankPostalCode = v.BankPostalCode,
                              BankState = v.BankState,
                              BankStreet1 = v.BankStreet1,
                              BankStreet2 = v.BankStreet2,
                              Currency = v.Currency,
                              Country = v.Country,
                              SwiftCode = v.SwiftCode,
                              ReferenceCode = v.ReferenceCode,
                              RoutingNumber = v.RoutingNumber,
                              Status = v.Status,
                          }).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccount> DetailsByNumberAsync(string accountNumber)
        {
            return await _context.VirtualAccounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccount> DetailsByBankAsync(long accountId, string bankName)
        {
            return await _context.VirtualAccounts.Where(x => x.AccountId == accountId && x.BankName == bankName).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccount> GetAsync(long accountId, string currency)
        {
            return await _context.VirtualAccounts.Where(x => x.AccountId == accountId && x.Currency == currency).FirstOrDefaultAsync();
        }

        public async Task<VirtualAccount> GetAsync(long accountId, string currency, string accountNumber)
        {
            return await _context.VirtualAccounts.Where(x => x.AccountId == accountId && x.Currency == currency && x.AccountNumber == accountNumber).FirstOrDefaultAsync();
        }
    }
}
