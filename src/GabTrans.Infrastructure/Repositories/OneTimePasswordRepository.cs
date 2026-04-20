using GabTrans.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Models;
using GabTrans.Application.Abstractions.Logging;

namespace GabTrans.Infrastructure.Repositories
{
    public class OneTimePasswordRepository(GabTransContext context, ILogService logService) : IOneTimePasswordRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> CreateAsync(long userId, string token, long categoryId)
        {
            var otp = new OneTimePassword()
            {
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(StaticData.OtpLifetime),
                Used = false,
                Token = token,
                OtpCategoryId = categoryId,
                UserId = userId,
            };
            _context.OneTimePasswords.Add(otp);
            await _context.SaveChangesAsync();
            return otp.Id > 0;
        }

        public async Task<OneTimePassword> DetailsAsync(long userId, string password)
        {
            return await _context.OneTimePasswords.Where(x => x.Token == password && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<OneTimePassword> DetailsAsync(string password)
        {
            return await _context.OneTimePasswords.Where(x => x.Token == password).FirstOrDefaultAsync();
        }

        public async Task<OneTimePassword> DetailsAsync(string password, long categoryId)
        {
            return await _context.OneTimePasswords.Where(x => x.Token == password && x.OtpCategoryId == categoryId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(long id, bool isUsed)
        {
            var otp = await _context.OneTimePasswords.Where(x => x.Id == id).FirstOrDefaultAsync();
            otp.Used = isUsed;
            otp.UsedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return otp.Id > 0;
        }


        public async Task<bool> UpdateAsync(OneTimePassword oneTimePassword)
        {
            try
            {
                _context.OneTimePasswords.Update(oneTimePassword);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(OneTimePasswordRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateKycAsync(long userId, long categoryId)
        {
            var kyc = await _context.Kycs.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (categoryId == (long)OTPCategories.EmailVerification) kyc.VerifyEmail = true;
            // if (categoryId == (long)OTPCategories.PhoneVerification) kyc.PhoneVerified = true;
            await _context.SaveChangesAsync();
            return kyc.Id > 0;
        }

        public async Task<bool> DeactivateAsync(long userId, long otpCategoryId)
        {
            var otp = await _context.OneTimePasswords.FirstOrDefaultAsync(x => x.UserId == userId && x.OtpCategoryId == otpCategoryId && !x.Used);
            otp.Used = true;
            otp.UsedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return otp.Id > 0;
        }

        public async Task<OneTimePassword> GetUnusedPasswordAsync(long userId, long categoryId)
        {
            return await _context.OneTimePasswords.FirstOrDefaultAsync(x => x.UserId == userId && x.OtpCategoryId == categoryId && !x.Used);
        }
    }
}
