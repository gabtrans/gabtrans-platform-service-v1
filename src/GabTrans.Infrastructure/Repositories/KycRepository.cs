using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Logging;
using NPOI.SS.Formula.Functions;

namespace GabTrans.Infrastructure.Repositories
{
    public class KycRepository(ILogService logService, GabTransContext context) : IKycRepository
    {
        private readonly ILogService _logService = logService;
        private readonly GabTransContext _context = context;

        public async Task<IEnumerable<AccountType>> GetAccountTypesAsync()
        {
            return await _context.AccountTypes.ToListAsync();
        }

        public async Task<IEnumerable<Kyc>> GetKycsAsync()
        {
            return await _context.Kycs.Where(x => x.Status == "").ToListAsync();
        }

        public async Task<Kyc> DetailsAsync(long id)
        {
            return await _context.Kycs.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Kyc> DetailsByUuidAsync(string uuid)
        {
            return await _context.Kycs.Where(x => x.Uuid == uuid).FirstOrDefaultAsync();
        }

        public async Task<Kyc> DetailsByUserAsync(long userId)
        {
            return await _context.Kycs.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Kyc> DetailsByTaxNumberAsync(string taxNumber)
        {
            return await _context.Kycs.Where(x => x.TaxNumber == taxNumber).FirstOrDefaultAsync();
        }

        public async Task<CompleteKycObject> GetKycDetailsAsync(long userId)
        {
            var query = await (from u in _context.Users
                               //from ua in _mobileContext.UserAccounts.AsNoTracking().Where(ua => ua.UserId == u.Id).DefaultIfEmpty()
                               from a in _context.Accounts.AsNoTracking().Where(a => a.UserId == u.Id).DefaultIfEmpty()
                               from k in _context.Kycs.AsNoTracking().Where(k => k.UserId == a.UserId).DefaultIfEmpty()
                                   //from ub in _mobileContext.UserBusinesses.AsNoTracking().Where(ub => ub.UserId == ua.UserId).DefaultIfEmpty()
                               from b in _context.Businesses.AsNoTracking().Where(b => b.UserId == u.Id).DefaultIfEmpty()
                               from bs in _context.Industries.AsNoTracking().Where(bs => bs.Name == b.MainIndustry).DefaultIfEmpty()
                               where u.Id.Equals(userId)

                               select new CompleteKycObject
                               {
                                   // AccountTypeId = a == null ? 0 : a.AccountTypeId,
                                   // AccountId = a == null ? 0 : a.Id,
                                   // BusinessName = b.Name ?? null,
                                   // BusinessNumber = b.RegistrationNumber ?? null,
                                   // BusinessType = b.BusinessType ?? null,
                                   // DateOfIncorporation = b.DateOfIncorporation ?? null,
                                   // EmailAddress = u.EmailAddress,
                                   // IsBusinessRegulated = b == null ? false : b.IsBusinessRegulated,
                                   // NatureOfBusinessId = b == null ? 0 : b.BusinessSectorId,
                                   // OperatingAddress = b.OperatingAddress ?? null,
                                   // OperatingCity = b.OperatingCity ?? null,
                                   // OperatingStreet = b.OperatingStreet ?? null,
                                   // RegistrationBodyId = b.RegistrationBodyId ?? null,
                                   // RegisteredAddress = b.RegisteredAddress ?? null,
                                   // //TransactionVolume = a.TransactionVolume ?? null,
                                   // //TransactionValue = a.TransactionValue ?? null,
                                   // JobBrief = k.JobBrief ?? null,
                                   // OccupationId = k.ProfessionId ?? null,
                                   // IdentificationType = k.IdentificationType ?? null,
                                   // PurposeOfAccount = a.PurposeOfAccount ?? null,
                                   //// DestinationOfFund = a.DestinationOfFunds ?? null,
                                   // FirstName = u.FirstName,
                                   // PhoneNumber = u.LastName,
                                   // PostalCode = k.PostalCode,
                                   // LastName = u.LastName,
                                   // DateOfBirth = k.DateOfBirth,
                                   // ResidentialAddress = k.ResidentialAddress,
                                   // ResidentialCity = k.ResidentialCity
                               }).FirstOrDefaultAsync();
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.PurposeOfAccount)) query.PurposeOfAccounts = GetArrayfromValue(query.PurposeOfAccount);
                if (!string.IsNullOrEmpty(query.DestinationOfFund)) query.DestinationOfFunds = GetArrayfromValue(query.DestinationOfFund);
            }
            return query;
        }


        public string[] GetArrayfromValue(string value)
        {
            string[] array = value.Split(',');
            return array;
        }


        public async Task<bool> UpdateAsync(Kyc kyc)
        {
            try
            {
                _context.Kycs.Update(kyc);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<IEnumerable<Kyc>> GetAsync(string status, string country)
        {
            return await (from a in _context.KycRequests.DefaultIfEmpty()
                          from k in _context.Kycs.Where(k => k.UserId == a.UserId).DefaultIfEmpty()
                          where a.Status == status && k.Status == status && k.Country != country
                          select k
                          ).ToListAsync();
        }

        public async Task<IEnumerable<Kyc>> GetCompletedAsync(string status, string country)
        {
            return await (from a in _context.KycRequests.DefaultIfEmpty()
                          from k in _context.Kycs.Where(k => k.UserId == a.UserId).DefaultIfEmpty()
                          where a.Status == KycStatuses.Approved && k.Status == status && k.Country != country && k.DataUploaded && k.DocumentUploaded
                          select k
                          ).ToListAsync();
        }
    }
}
