using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using Newtonsoft.Json;
using System.Drawing;
using GabTrans.Application.Abstractions.Logging;

namespace GabTrans.Infrastructure.Repositories
{
    public class RepresentativeRepository(GabTransContext context, ILogService logService) : IRepresentativeRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<RepresentativeModel> DetailsAsync(long Id)
        {
            return await _context.Kycs.Where(x => x.Id == Id).Select(a => new RepresentativeModel
            {
                //Address = a.Address,
                //BusinessId = a.BusinessId,
                //CountryCode = a.CountryCode,
                //CountryOfResidence = a.CountryOfResidence,
                //CreatedOn = a.CreatedAt,
                //EmailAddress = a.EmailAddress,
                //FirstName = a.FirstName,
                //HasAdverseMedia = a.AdverseMedia,
                //IdentificationBack = a.IdentificationBack,
                //IdentificationFront = a.IdentificationFront,
                //IdentificationNumber = a.IdentificationNumber,
                //IsPep = a.Pep,
                //IsPrincipal = a.Principal,
                //IsScreened = a.Screened,
                //IsVerified = a.Verified,
                //LastName = a.LastName,
                //MiddleName = a.MiddleName,
                //PhoneNumber = a.PhoneNumber,
                //Occupation = a.Occupation,
                //Outcome = a.Outcome,
                //RiskScore = a.RiskScore,
                //SelfieFront = a.Selfie,
                //Business = a.Business.Name,
                //IsPushed = a.Pushed,
               // Role = a.Role,
                DateOfBirth = a.DateOfBirth
            }).FirstOrDefaultAsync();
        }

        public async Task<Representative> DetailsByIdAsync(long Id)
        {
            return await _context.Kycs.Where(x => x.Id == Id).Select(x => new Representative { }).FirstOrDefaultAsync();
        }


        public async Task<List<Representative>> GetByBusinessAsync(long businessId)
        {
            return await _context.Kycs.Where(x => x.Id == businessId).Select(x=>new Representative { }).ToListAsync();
        }

        public async Task<List<RepresentativeModel>> GetAsync(GetRepresentativeRequest request)
        {
            return await (from d in _context.Kycs.AsNoTracking()
                         // from b in _context.Businesses.AsNoTracking().Where(b => b.Id == d.BusinessId)
                              //from u in _context.UserBusinesses.AsNoTracking().Where(u => u.BusinessId == d.BusinessId)
                         // where (d.CreatedAt >= startDate && d.CreatedAt <= endDate) && b.Identifier.Equals(companyNumber) && b.UserId.Equals(userId)
                          select new RepresentativeModel
                          {
                              Id = d.Id,
                            //  BusinessId = d.BusinessId,
                              //Address = d.Address,
                              //Business = b.Name,
                              //CountryCode = d.CountryCode,
                              //CountryOfResidence = d.CountryOfResidence,
                              //CreatedOn = d.CreatedAt,
                              //EmailAddress = d.EmailAddress,
                              //FirstName = d.FirstName,
                              //HasAdverseMedia = d.AdverseMedia,
                              //IdentificationBack = d.IdentificationBack,
                              //IdentificationFront = d.IdentificationFront,
                              //IdentificationNumber = d.IdentificationNumber,
                              //IsPep = d.Pep,
                              //IsPrincipal = d.Principal,
                              //LastName = d.LastName,
                              //MiddleName = d.MiddleName,
                              //Occupation = d.Occupation,
                              //PhoneNumber = d.PhoneNumber,
                              //RiskScore = d.RiskScore,
                              //IsVerified = d.Verified,
                              //SelfieFront = d.Selfie,
                              //Outcome = d.Outcome,
                              //IsScreened = d.Screened,
                              //IsPushed = d.Pushed,
                            //  Role = d.Role,
                              DateOfBirth = d.DateOfBirth
                          }).ToListAsync();
        }

        public async Task<List<RepresentativeModel>> GetAsync(long BusinessId)
        {
            return await (from d in _context.Kycs.AsNoTracking()
                         // from b in _context.Businesses.AsNoTracking().Where(b => b.Id == d.BusinessId)
                              //from u in _context.UserBusinesses.AsNoTracking().Where(u => u.BusinessId == d.BusinessId)
                              // where (d.CreatedAt >= startDate && d.CreatedAt <= endDate) && b.Identifier.Equals(companyNumber) && b.UserId.Equals(userId)
                          select new RepresentativeModel
                          {
                              Id = d.Id,
                            //  BusinessId = d.BusinessId,
                              //Address = d.Address,
                              //Business = b.Name,
                              //CountryCode = d.CountryCode,
                              //CountryOfResidence = d.CountryOfResidence,
                              //CreatedOn = d.CreatedAt,
                              //EmailAddress = d.EmailAddress,
                              //FirstName = d.FirstName,
                              //HasAdverseMedia = d.AdverseMedia,
                              //IdentificationBack = d.IdentificationBack,
                              //IdentificationFront = d.IdentificationFront,
                              //IdentificationNumber = d.IdentificationNumber,
                              //IsPep = d.Pep,
                              //IsPrincipal = d.Principal,
                              //LastName = d.LastName,
                              //MiddleName = d.MiddleName,
                              //Occupation = d.Occupation,
                              //PhoneNumber = d.PhoneNumber,
                              //RiskScore = d.RiskScore,
                              //IsVerified = d.Verified,
                              //SelfieFront = d.Selfie,
                              //Outcome = d.Outcome,
                              //IsScreened = d.Screened,
                              //IsPushed = d.Pushed,
                             // Role = d.Role,
                              DateOfBirth = d.DateOfBirth
                          }).ToListAsync();
        }

        public async Task<bool> InsertAsync(Representative representative)
        {
            try
            {
               // await _context.Representatives.Add(representative);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(RepresentativeRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Representative representative)
        {
            try
            {
               // _context.Representatives.Update(representative);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(RepresentativeRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
