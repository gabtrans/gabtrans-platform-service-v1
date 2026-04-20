using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula;

namespace GabTrans.Infrastructure.Repositories
{
    public class CountryRepository(GabTransContext context, ILogService logService) : ICountryRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<List<IdNameObject>> GetCitiesAsync(string countryCode)
        {
            return await (
                from c in _context.Cities.AsNoTracking()
                from s in _context.Countries.Where(s => s.Code == c.CountryCode).OrderBy(x => x.Name)
                where s.Code2.Equals(countryCode)
                select new IdNameObject
                {
                    Name = c.Name,
                    Id = c.Id,
                    Description = c.Name
                }
                ).ToListAsync();
        }

        public async Task<List<CurrencyModel>> GetCountriesAsync()
        {
            return await _context.Countries.Select(u => new CurrencyModel
            {
                Id = u.Id,
                Code = u.Code,
                Flag = $"{StaticData.AppUrl}/img/flag/{u.Flag}",
                Name = u.Name
            }).ToListAsync();
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<List<Country>> GetCountriesAsync(List<string> codes)
        {
            return await _context.Countries.Where(x => codes.Contains(x.Code)).ToListAsync();
        }

        public async Task<List<CurrencyModel>> GetCurrenciesAsync(string countryCode)
        {
            return await (from c in _context.Currencies.AsNoTracking()
                          from cc in _context.Countries.AsNoTracking().Where(cc => cc.Code == c.CountryCode).OrderBy(x => x.Name)
                          where c.CountryCode.Equals(countryCode)
                          select new CurrencyModel
                          {
                              Id = c.Id,
                              Code = c.Code,
                              //Flag = cc.Flag,
                              //Name = cc.CurrencyName,
                              CountryCode = countryCode,
                              // Symbol = c.Symbol,
                              // SubsidiaryCode = c.SubsidiaryCode
                          }).ToListAsync();
        }

        public async Task<List<ProvinceObject>> GetProvincesAsync(string countryCode)
        {
            return await _context.Provinces.Where(x => x.CountryCode == countryCode).OrderBy(x => x.Name).Select(a => new ProvinceObject { Name = a.Name, CountryCode = a.CountryCode, Id = a.Id }).ToListAsync();
        }

        public async Task<CountryObject> DetailsAsync(string countryCode)
        {
            return await _context.Countries.Where(c => c.Code == countryCode).Select(u => new CountryObject
            {
                Id = u.Id,
                Code = u.Code,
                Code2 = u.Code2,
                Name = u.Name
            }).FirstOrDefaultAsync();
        }

        public async Task<List<NameCodeObject>> GetStatesAsync(string countryCode)
        {
            return await _context.States.Where(c => c.CountryCode == countryCode).Select(u => new NameCodeObject
            {
                Name = u.Name,
                Code = u.Code
            }).ToListAsync();
        }

        public async Task<Country> GetCountryDetailsAsync(string countryCode)
        {
            return await _context.Countries.Where(x => x.Code == countryCode).FirstOrDefaultAsync();
        }

        public async Task<List<IdNameObject>> GetRegionsAsync(string countryCode)
        {
            return await _context.Provinces.OrderBy(x => x.Name).Select(u => new IdNameObject
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Name
            }).ToListAsync();
        }

        public async Task<List<IdNameObject>> GetLocalGovernmentAsync(int state)
        {
            return await _context.Cities.Where(c => c.Id == state).OrderBy(x => x.Name).Select(u => new IdNameObject
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Name
            }).ToListAsync();
        }

        public async Task<List<Continent>> GetContinentsAsync()
        {
            return await _context.Continents.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
