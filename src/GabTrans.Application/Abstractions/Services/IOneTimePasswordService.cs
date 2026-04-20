using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IOneTimePasswordService
    {
        Task<ApiResponse> LockAsync(string otp);
        Task DeactivateAsync(long userId, long otpCategoryId);
        Task<ApiResponse> GenerateAsync(long userId, long otpCategoryId);
        Task<ApiResponse> ValidateAsync(long userId, long otpCategoryId, string otp);
    }
}
