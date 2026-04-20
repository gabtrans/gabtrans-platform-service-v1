using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Logging;

namespace GabTrans.Infrastructure.Repositories
{
    public class BusinessRepository(GabTransContext context, ILogService logService) : IBusinessRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<BusinessObject> DetailsByAccountIdAsync(long accountId)
        {
            return await (from a in _context.Accounts.AsNoTracking()
                              // from ua in _context.UserAccounts.AsNoTracking().Where(ua => ua.AccountId == a.Id).DefaultIfEmpty()
                              //from ub in _context.UserBusinesses.AsNoTracking().Where(ub => ub.UserId == a.UserId).DefaultIfEmpty()
                          from b in _context.Businesses.AsNoTracking().Where(b => b.UserId == a.UserId).DefaultIfEmpty()
                          where a.Id.Equals(accountId)
                          select new BusinessObject
                          {
                              Id = b.Id,
                              //LicenseNumber = b.RegistrationNumber,
                              //BusinessType = b.BusinessType,
                              //BusinessSectorId = b.BusinessSectorId,
                              //DateOfIncorporation = b.DateOfIncorporation,
                              //CertificateBack = b.CertificateBack,
                              //CertificateFront = b.CertificateFront,
                              //Introduction = b.Introduction,
                              //Name = b.Name,
                              //IsActive = b.Active,
                              //OperatingAddress = b.OperatingAddress,
                              //OperatingCity = b.OperatingCity,
                              //OperatingStreet = b.OperatingStreet,
                              //PostalCode = b.PostalCode,
                              //RegisteredAddress = b.RegisteredAddress,
                              //RegistrationBodyId = b.RegistrationBodyId
                          }).FirstOrDefaultAsync();
        }

        public async Task<bool> DoesNameExistAsync(string name)
        {
            return await _context.Businesses.AnyAsync(x => x.Name == name);
        }

        public async Task<List<BusinessType>> GetTypesAsync()
        {
            return await _context.BusinessTypes.OrderBy(b => b.Name).ToListAsync();
        }

        public async Task<List<BusinessRole>> GetRolesAsync()
        {
            return await _context.BusinessRoles.OrderBy(b => b.Name).ToListAsync();
        }

        public async Task<Business> DetailsByIdAsync(long id)
        {
            return await _context.Businesses.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Business> DetailsAsync(string companyNumber)
        {
            return await _context.Businesses.Where(x => x.Identifier == companyNumber).FirstOrDefaultAsync();
        }

        public async Task<Business> GetByUserAsync(long userId)
        {
            return await _context.Businesses.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(Business business)
        {
            try
            {
                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(BusinessRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<long> CreateAsync(Business business)
        {
            try
            {
                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();
                return business.Id;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(BusinessRepository), nameof(CreateAsync), ex);
            }

            return 0;
        }

        public async Task<bool> UpdateAsync(Business business)
        {
            try
            {
                _context.Update(business);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(BusinessRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<BusinessAddressModel> GetAddressAsync(long userId)
        {
            return await (from b in _context.Businesses.AsNoTracking()
                          from s in _context.States.AsNoTracking().Where(s => s.Code == b.State && s.CountryCode == b.Country).DefaultIfEmpty()
                          from c in _context.Countries.AsNoTracking().Where(c => c.Code == b.Country).DefaultIfEmpty()
                          from ms in _context.States.AsNoTracking().Where(ms => ms.Code == b.MailingState && ms.CountryCode == b.MailingCountry).DefaultIfEmpty()
                          from mc in _context.Countries.AsNoTracking().Where(mc => mc.Code == b.MailingCountry).DefaultIfEmpty()
                          where b.UserId == userId
                          select new BusinessAddressModel
                          {
                              BusinessCity = b.City,
                              MailingState = ms.Name,
                              BusinessCountry = c.Name,
                              BusinessLine1 = b.Address1,
                              BusinessLine2 = b.Address2,
                              BusinessPostalCode = b.PostalCode,
                              BusinessState = s.Name,
                              MailingCity = b.MailingCity,
                              MailingCountry = mc.Name,
                              MailingLine1 = b.MailingAddress1,
                              MailingLine2 = b.MailingAddress2,
                              MailingPostalCode = b.MailingPostalCode
                          }).FirstOrDefaultAsync();
        }

        public async Task<BusinessDocumentModel> GetDocumentAsync(long userId)
        {
            return await _context.Businesses.AsNoTracking().Where(b => b.UserId == userId).Select(b => new BusinessDocumentModel
            {
                Agreement = b.Agreement,
                BankStatement = b.BankStatement,
                FormationDocument = b.FormationDocument,
                ProofOfOwnership = b.ProofOfOwnership,
                ProofOfRegistration = b.ProofOfRegistration,
                TaxDocument = b.TaxDocument
            }).FirstOrDefaultAsync();
        }

        public async Task<BusinessInformationModel> GetInformationAsync(long userId)
        {
            return await _context.Businesses.AsNoTracking().Where(b => b.UserId == userId).Select(b => new BusinessInformationModel
            {
                Website = b.Website,
                CurrenciesNeeded = b.CurrencyNeeded,
                Description = b.Description,
                Identifier = b.Identifier,
                IncorporationDate = (DateTime)b.IncorporationDate,
                MainIndustry = b.MainIndustry,
                MonthlyConversionVolumeDigitalAssets = b.MonthlyConversionVolumeDigitalAssets,
                MonthlyRevenue = b.MonthlyRevenue,
                MonthlySWIFTVolume = b.MonthlySwiftVolume,
                NAICS = b.Naics,
                NAICSDescription = b.NaicsDescription,
                Name = b.Name,
                TradeName = b.TradeName,
                MonthlyLocalPaymentVolume = b.MonthlyLocalPaymentVolume,
                Type = b.Type,
                MonthlyConversionVolumeFiat = b.MonthlyConversionVolumeFiat,
                CountriesOfOperation = b.CountriesOfOperation
            }).FirstOrDefaultAsync();
        }
    }
}
