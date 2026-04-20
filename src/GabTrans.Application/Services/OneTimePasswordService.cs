using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;

namespace GabTrans.Application.Services
{
    public class OneTimePasswordService(ILogService logService, ISequenceService sequenceService, IValidationService validationService, IOneTimePasswordRepository oneTimePasswordRepository) : IOneTimePasswordService
    {
        private readonly ILogService _logService = logService;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IValidationService _validationService = validationService;
        private readonly IOneTimePasswordRepository _oneTimePasswordRepository = oneTimePasswordRepository;

        public async Task<ApiResponse> GenerateAsync(long userId, long otpCategoryId)
        {
            var result = new ApiResponse();
            try
            {
                await DeactivateAsync(userId, otpCategoryId);

                string token = _sequenceService.GenerateRandomNumber();

                await _oneTimePasswordRepository.CreateAsync(userId, token, otpCategoryId);

                result.Success = true;

                result.Data = token;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                _logService.LogError("OneTimePasswordService", "Generate", ex);

                result.Message = "Kindly try again later";
            }

            return result;
        }

        public async Task DeactivateAsync(long userId, long otpCategoryId)
        {
            var otp = await _oneTimePasswordRepository.GetUnusedPasswordAsync(userId, otpCategoryId);
            if (otp is not null)
            {
                await _oneTimePasswordRepository.DeactivateAsync(userId, otpCategoryId);
            }
        }

        public async Task<ApiResponse> ValidateAsync(long userId, long otpCategoryId, string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the OTP"
                };
            }

            var userToken = await _oneTimePasswordRepository.DetailsAsync(otp, otpCategoryId);
            if (userToken == null)
            {
                return new ApiResponse
                {
                    Message = "Invalid OTP"
                };
            }

            if (userToken.Used)
            {
                return new ApiResponse
                {
                    Message = "OTP has been used"
                };
            }

            var diff = _validationService.MinuteDiff(DateInterval.Minute, userToken.CreatedAt, DateTime.Now);
            if (diff > StaticData.OtpLifetime)
            {
                return new ApiResponse
                {
                    Message = "OTP has expired"
                };
            }

            return new ApiResponse { Success = true, Message = "Otp updated successfully" };
        }

        public async Task<ApiResponse> LockAsync(string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the OTP"
                };
            }

            var oneTimePassword = await _oneTimePasswordRepository.DetailsAsync(otp);
            if (oneTimePassword == null)
            {
                return new ApiResponse
                {
                    Message = "Invalid OTP"
                };
            }

            if (oneTimePassword.Used)
            {
                return new ApiResponse
                {
                    Message = "OTP has been used"
                };
            }

            oneTimePassword.Used = true;
            oneTimePassword.UsedOn = DateTime.Now;
            bool update = await _oneTimePasswordRepository.UpdateAsync(oneTimePassword);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update the OTP"
                };
            }

            return new ApiResponse { Success = true };
        }
    }
}
