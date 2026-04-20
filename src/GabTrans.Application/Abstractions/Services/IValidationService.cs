using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Enums;
using System.Data.Common;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IValidationService
    {
        bool IsEmailValid(string emailaddress);
        bool IsMobileNumberValid(string mobileNumber);
        bool IsPinValid(string pin);
        bool IsURLValid(string url);
        bool IsBVNValid(string bvn);
        bool IsPasswordValid(string password);
        string GetString(DbDataReader reader, string ordinal);
        DateTime? GetDate(DbDataReader reader, string ordinal);
        long GetLong(DbDataReader reader, string ordinal);
        ApiResponse UpdateAuthorizationPin(UpdateAuthorizationPinRequest request, long userId);
        ApiResponse CompleteSignUp(CompleteSignUpRequest request);
        decimal GetDecimal(DbDataReader reader, string ordinal);
        bool GetBoolean(DbDataReader reader, string ordinal);
        ApiResponse ValidatePassword(string password);
        string GetPhoneNumber(string phoneNumber, string countryCode);
        ApiResponse ConfirmPhoneNumber(ConfirmPhoneNoRequest request);
        ApiResponse SignIn(SignInRequest request);
        ApiResponse InitiateSignUp(InitiateSignUpRequest request);
        ApiResponse ConfirmEmailAddress(ConfirmEmailAddressRequest request);
        ApiResponse CompleteOnboarding(CompleteOnboardingRequest request);
        ApiResponse CompleteKyc(CompleteKycRequest request);
        ApiResponse CreateCustomerAccount(CompleteOnboardingRequest request);
        ApiResponse BusinessOnboarding(BusinessOnboardingRequest request);
        ApiResponse GetRecieverRequirement(GetBeneficiaryRequirement beneficiaryRequirement);
        ApiResponse Trade(TradeCryptoRequest request);
        ApiResponse Transfer(TransferRequest request);
        ApiResponse InternalTransfer(InternalTransferRequest request);
        ApiResponse ValidateInvitation(InvitationRequest request, long businessId);
        ApiResponse ValidateUser(CreateUserRequest request);
        ApiResponse SendSms(SendSmsRequest request);
        ApiResponse BVNLookUp(string bvn);
        ApiResponse DirectorBVNLookUp(ValidateDirectorBvnRequest request);
        ApiResponse SetupPin(PinRequest request);
        ApiResponse UpdatePin(PinRequest request);
        long DateDiff(DateInterval interval, DateTime date1, DateTime date2);
        ApiResponse VerifyPhoneNumber(PhoneVerificationRequest request, string countryCode);
        ApiResponse FinalizeOnboarding(FinalizeOnboardingRequest request);
        long MinuteDiff(DateInterval interval, DateTime date1, DateTime date2);
        ApiResponse UpdateBusinessAddress(UpdateBusinessAddressRequest request, long userId);
        ApiResponse UpdateBusinessDocument(UpdateBusinessDocumentRequest request, long userId);
        ApiResponse UpdateBusinessInformation(UpdateBusinessInformationRequest request, long userId);
        ApiResponse UpdateGeneralInformation(UpdateGeneralInformationRequest request, long userId);
        ApiResponse UpdatePersonal(UpdatePersonalRequest request, long userId);
        ApiResponse UpdateEmployment(UpdateEmploymentRequest request, long userId);
        ApiResponse UpdateAddress(UpdateAddressRequest request, long userId);
        ApiResponse UpdateIdentity(UpdateIdentityRequest request, long userId);
        ApiResponse UpdateIncome(UpdateIncomeRequest request, long userId);
        ApiResponse TradeFx(TradeFxRequest request, long userId);
    }
}
