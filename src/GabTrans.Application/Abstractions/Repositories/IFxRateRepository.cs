using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IFxRateRepository
    {
        Task<List<FxRate>> GetRatesAsync(string from, string type);
        Task<FxRate> GetRateAsync(string from, string to);
        Task<FxRate> GetRateAsync(string from, string to, string type);
        Task<FxRate> GetRateAsync(string from, string to, long accountId);
        Task<bool> InsertAsync(FxRate fxRate);
        Task<IEnumerable<ExchangeRateModel>> GetAsync(QueryFxRate queryFxRate);
        Task<FxRate> DetailsAsync(long id);
        Task<bool> UpdateAsync(FxRate fxRate);
        Task<(IEnumerable<FxRate> foundRates, List<long> missingIds)> GetAsync(List<long> rateIds);
        Task<List<FxRate>> GetExistingRates(List<(string BaseCurrency, string TargetCurrency)> currencyPairs);
    }
}
