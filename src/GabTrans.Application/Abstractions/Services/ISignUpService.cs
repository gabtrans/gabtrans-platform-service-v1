using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;


namespace GabTrans.Application.Abstractions.Services
{
    public interface ISignUpService
    {
     //   Task CreateAccountAsync();
        Task TestMethod();
       // Task<ApiResponse> JumioOnboardingAsync(long userId, string referenceNumber, string identificationType, long channelId, string ipAddress, long? businessId = null);
      //  Task<string> JumioDirectorOnboardingAsync(long directorId, string referenceNumber, Channels channel, string ipAddress);
       // Task<ApiResponse> ValidateBvnAsync(string firstName, string lastName, DateTime dateOfBirth, string bvn);
       // Task<ApiResponse> ValidateBvnAsync(long userId, string firstName, string lastName, DateTime dateOfBirth, string bvn);
        Task<ApiResponse> EmailVerificationAsync(User user);
        //Task<ApiResponse> PhoneVerificationAsync(User user);
       // Task<ApiResponse> UpgradeKycAsync(long userId, string countryCode);
    }
}
