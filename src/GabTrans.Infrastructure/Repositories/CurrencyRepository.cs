using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;

namespace GabTrans.Infrastructure.Repositories
{
    public class CurrencyRepository(GabTransContext context) : ICurrencyRepository
    {
        private readonly GabTransContext _context = context;

        public async Task<CurrencyModel> DetailsAsync(long Id)
        {
            return await (from c in _context.Currencies.AsNoTracking()
                          from cc in _context.Countries.AsNoTracking().Where(cc => cc.Code == c.CountryCode)

                          select new CurrencyModel
                          {
                              Id = c.Id,
                              Code = c.Code,
                              //Flag = cc.Flag,
                              //Name = cc.CurrencyName
                          }).FirstOrDefaultAsync();
        }

        public async Task<Currency> DetailsByCodeAsync(string currencyCode)
        {
            return await _context.Currencies.Where(x => x.Code == currencyCode).FirstOrDefaultAsync();
        }

        public async Task<Currency> DetailsByCountryAsync(string countryCode)
        {
            return await _context.Currencies.Where(x => x.CountryCode == countryCode.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<List<Currency>> GetAsync()
        {
            return await _context.Currencies.AsNoTracking().ToListAsync();
        }

        public async Task<List<CurrencyModel>> GetAllAsync()
        {
            return await (from c in _context.Currencies.AsNoTracking()
                          from cc in _context.Countries.AsNoTracking().Where(cc => cc.Code == c.CountryCode).DefaultIfEmpty()

                          select new CurrencyModel
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Flag = $"{StaticData.AppUrl}/img/flag/{cc.Flag}",
                              Name = cc.Currency
                          }).OrderBy(x => x.Code).ToListAsync();
        }

        //public async Task<List<CurrencyObject>> PendingAsync(long accountId)
        //{
        //    var currencies = await GetAsync();
        //    var userCurrencies = await GetByAccountAsync(accountId);
        //    if (currencies != null && userCurrencies != null) currencies = currencies.Where(c => !userCurrencies.Any(x => x.Code == c.Code)).ToList();
        //    return currencies;
        //}

        // public async Task<TransactionHistoryModel> GetTransactionHistoryAsync()
        // {
        //     return await (from d in context.Deposits.AsNoTracking()
        //                   from a in context.Accounts.AsNoTracking().Where(a => a.Id == d.AccountId).DefaultIfEmpty()
        //                   from u in context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
        //                   where 
        //                   select new TransactionHistoryModel
        //                   {
        //                       AccountId = a.Id,
        //                       AccountName = a.Name,
        //                       Email = u.EmailAddress,
        //                       TransactionDate = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), // assuming p.CreatedAt exists
        //                       Reference = d.ReferenceNumber,
        //                       TransactionType = TransactionTypes.Deposit,
        //                       Amount = d.Amount,
        //                       Status = d.Status
        //                   }).FirstOrDefaultAsync();
        // }
    }
}
