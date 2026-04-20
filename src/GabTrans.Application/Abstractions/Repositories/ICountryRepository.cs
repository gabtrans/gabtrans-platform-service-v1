using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllCountriesAsync();
        Task<List<IdNameObject>> GetCitiesAsync(string countryCode);
        Task<List<CurrencyModel>> GetCountriesAsync();
        Task<List<Country>> GetCountriesAsync(List<string> codes);
        Task<List<CurrencyModel>> GetCurrenciesAsync(string countryCode);
        Task<List<ProvinceObject>> GetProvincesAsync(string countryCode);
        Task<CountryObject> DetailsAsync(string countryCode);
        Task<Country> GetCountryDetailsAsync(string countryCode);
        Task<List<NameCodeObject>> GetStatesAsync(string countryCode);
        Task<List<IdNameObject>> GetRegionsAsync(string countryCode);
        Task<List<IdNameObject>> GetLocalGovernmentAsync(int state);
        Task<List<Continent>> GetContinentsAsync();
    }
}
