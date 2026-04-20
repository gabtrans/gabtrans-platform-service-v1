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
    public class FxRateRepository(GabTransContext context, ILogService logService) : IFxRateRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(FxRate fxRate)
        {
            try
            {
                _context.FxRates.Add(fxRate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxRateRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetAsync(QueryFxRate queryFxRate)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryFxRate.StartDate) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(queryFxRate.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryFxRate.EndDate) ? DateTime.Now : Convert.ToDateTime(queryFxRate.EndDate);

            return await (from a in _context.FxRates.AsNoTracking()
                          where
                            (string.IsNullOrEmpty(queryFxRate.Currency) || a.FromCurrency == queryFxRate.Currency || a.ToCurrency == queryFxRate.Currency)
                              && a.CreatedAt.Date >= fromDate.Date && a.CreatedAt.Date <= toDate.Date
                          select new ExchangeRateModel
                          {
                              Id = a.Id,
                              FriendlyDisplayAmount = a.Id,
                              Rate = a.Rate,
                              Type = a.Type,
                              CreatedAt = a.CreatedAt,
                              Status = a.Status,
                              RateMarkUp = a.RateMarkUp,
                              RateFromProvider = a.RateFromProvider,
                              From = a.FromCurrency,
                              To = a.ToCurrency
                          }).ToListAsync();
        }

        public async Task<bool> UpdateAsync(FxRate fxRate)
        {
            try
            {
                _context.FxRates.Update(fxRate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxRateRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<(IEnumerable<FxRate> foundRates, List<long> missingIds)> GetAsync(List<long> rateIds)
        {
            var foundRates = await _context.FxRates.Where(x => rateIds.Contains(x.Id)).ToListAsync();

            var foundIds = foundRates.Select(x => x.Id).ToHashSet();
            var missingIds = rateIds.Where(id => !foundIds.Contains(id)).ToList();

            return (foundRates, missingIds);
        }


        public async Task<List<FxRate>> GetExistingRates(List<(string BaseCurrency, string TargetCurrency)> currencyPairs)
        {
            var baseCurrencies = currencyPairs.Select(x => x.BaseCurrency).ToList();
            var targetCurrencies = currencyPairs.Select(x => x.TargetCurrency).ToList();

            return await _context.FxRates.Where(rate => baseCurrencies.Contains(rate.FromCurrency) && targetCurrencies.Contains(rate.ToCurrency)).ToListAsync();
        }

        public async Task<FxRate> DetailsAsync(long id)
        {
            return await _context.FxRates.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<FxRate>> GetRatesAsync(string from, string type)
        {
            return await _context.FxRates.Where(x => x.FromCurrency == from && x.Status == "active").ToListAsync();
        }

        public async Task<FxRate> GetRateAsync(string from, string to)
        {
            return await _context.FxRates.Where(x => x.FromCurrency == from && x.ToCurrency == to && x.Status == "active").FirstOrDefaultAsync();
        }

        public async Task<FxRate> GetRateAsync(string from, string to, long accountId)
        {
            var fxRate = await _context.FxRates.Where(x => x.FromCurrency == from && x.ToCurrency == to && x.Status == "active" && x.AccountId == accountId).FirstOrDefaultAsync();
            if (fxRate is not null) return fxRate;

            return await _context.FxRates.Where(x => x.FromCurrency == from && x.ToCurrency == to && x.Status == "active").FirstOrDefaultAsync();
        }

        public async Task<FxRate> GetRateAsync(string from, string to, string type)
        {
            return await _context.FxRates.Where(x => x.FromCurrency == from && x.ToCurrency == to && x.Type == type && x.Status == "active").FirstOrDefaultAsync();
        }
    }
}
