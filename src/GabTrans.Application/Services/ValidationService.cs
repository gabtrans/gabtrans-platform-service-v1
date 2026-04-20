using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Constants;
using System.Data;
using System.Data.Common;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Enums;

namespace GabTrans.Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly ILogService _logService;

        public ValidationService(ILogService logManager)
        {
            _logService = logManager;
        }

        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                var addr = new MailAddress(emailaddress);
                return addr.Address == emailaddress;
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsEmailValid", ex);
                return false;
            }
        }

        public bool IsMobileNumberValid(string mobileNumber)
        {
            try
            {
                return Regex.Match(mobileNumber, @"^([0-9]{11}|(^.))$").Success;
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsMobileNumberValid", ex);
                return false;
            }
        }


        public bool IsPinValid(string pin)
        {
            try
            {
                return Regex.Match(pin, @"^([0-9]{6}|(^.))$").Success;
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsPinValid", ex);
                return false;
            }
        }


        public bool IsURLValid(string url)
        {
            try
            {
                string Pattern = @"^(?:http|http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
                Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return Rgx.IsMatch(url);
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsURLValid", ex);
            }
            return false;
        }

        public bool IsBVNValid(string bvn)
        {
            try
            {
                return Regex.Match(bvn, @"^([0-9]{11}|(^.))$").Success;
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsBVNValid", ex);
                return false;
            }
        }

        public bool IsPasswordValid(string password)
        {
            try
            {
                if (password.Length < 8) return false;
                //if (password.Any(char.IsUpper) && password.Any(char.IsDigit) && Regex.Match(password, @"^[a-zA-Z0-9 ]*$").Success) return true;
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$");
                return regex.IsMatch(password);
            }
            catch (Exception ex)
            {
                _logService.LogError("ValidationService", "IsPasswordValid", ex);
            }
            return false;
        }


        public bool IsIpAddressValid(string ipAddress) =>
      new Regex("^(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
          .IsMatch(ipAddress);

        public string GetString(DbDataReader reader, string ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        public DateTime? GetDate(DbDataReader reader, string ordinal)
        {
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }

        public long GetLong(DbDataReader reader, string ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }

        public Decimal GetDecimal(DbDataReader reader, string ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDecimal(ordinal);
        }

        public bool GetBoolean(DbDataReader reader, string ordinal)
        {
            return reader.IsDBNull(ordinal) ? false : reader.GetBoolean(ordinal);
        }

        public string GetPhoneNumber(string phoneNumber, string countryCode)
        {
            phoneNumber = countryCode switch
            {
                Countries.China => DialingCodes.China + phoneNumber,
                Countries.Ghana => DialingCodes.Ghana + phoneNumber,
                Countries.United_Kingdom => DialingCodes.United_Kingdom + phoneNumber,
                Countries.Kenya => DialingCodes.Kenya + phoneNumber,
                _ => DialingCodes.Nigeria + phoneNumber,
            };
            return phoneNumber;
        }

        public ApiResponse UpdateBusinessDocument(UpdateBusinessDocumentRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid UserId"
                };
            }

            if (string.IsNullOrEmpty(request.BankStatement))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Bank statement"
                };
            }

            if (string.IsNullOrEmpty(request.FormationDocument))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Formation document"
                };
            }

            if (string.IsNullOrEmpty(request.ProofOfOwnership))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Proof of ownership"
                };
            }

            if (string.IsNullOrEmpty(request.ProofOfRegistration))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Proof of registration"
                };
            }

            if (string.IsNullOrEmpty(request.TaxDocument))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Tax document"
                };
            }

            if (string.IsNullOrEmpty(request.Agreement))
            {
                return new ApiResponse
                {
                    Message = "Please supply your agreement document"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateGeneralInformation(UpdateGeneralInformationRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid UserID"
                };
            }

            if (string.IsNullOrEmpty(request.MainIndustry))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Industry"
                };
            }

            //if (string.IsNullOrEmpty(request.AdditionalIndustry))
            //{
            //    return new ApiResponse
            //    {
            //        Message = " Invalid Industry address"
            //    };
            //}

            if (string.IsNullOrEmpty(request.NAICS))
            {
                return new ApiResponse
                {
                    Message = "Please supply your NAICS"
                };
            }

            if (string.IsNullOrEmpty(request.NAICSDescription))
            {
                return new ApiResponse
                {
                    Message = "Please supply your NAICS description"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateBusinessInformation(UpdateBusinessInformationRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid UserId"
                };
            }

            if (string.IsNullOrEmpty(request.CurrenciesNeeded))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Currencies"
                };
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Description"
                };
            }

            if (string.IsNullOrEmpty(request.Identifier))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Identifier"
                };
            }

            if (request.IncorporationDate == DateTime.MinValue)
            {
                return new ApiResponse
                {
                    Message = "Please supply your Incorporation Date"
                };
            }

            if (string.IsNullOrEmpty(request.MonthlyConversionVolumeDigitalAssets))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Monthly conversion volume digital assets"
                };
            }

            if (string.IsNullOrEmpty(request.MonthlyConversionVolumeFiat))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Monthly conversion volume fiat"
                };
            }

            if (string.IsNullOrEmpty(request.MonthlyLocalPaymentVolume))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Monthly local payment volume"
                };
            }

            if (string.IsNullOrEmpty(request.MonthlyRevenue))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Monthly revenue"
                };
            }

            if (string.IsNullOrEmpty(request.MonthlySWIFTVolume))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Monthly SWIFT volume"
                };
            }

            // if (string.IsNullOrEmpty(request.Name))
            // {
            //     return new ApiResponse
            //     {
            //         Message = " Invalid Name"
            //     };
            // }

            if (string.IsNullOrEmpty(request.TradeName))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Trade name"
                };
            }

            if (string.IsNullOrEmpty(request.Type))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Business Type"
                };
            }

            if (string.IsNullOrEmpty(request.Website))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Website"
                };
            }

            if (string.IsNullOrEmpty(request.CountriesOfOperation))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Country"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateBusinessAddress(UpdateBusinessAddressRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid UserID"
                };
            }

            if (string.IsNullOrEmpty(request.City))
            {
                return new ApiResponse
                {
                    Message = "Please supply your City"
                };
            }

            if (string.IsNullOrEmpty(request.Country))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Country"
                };
            }

            if (string.IsNullOrEmpty(request.Address1))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Address1"
                };
            }

            if (string.IsNullOrEmpty(request.Address2))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Address2"
                };
            }

            if (string.IsNullOrEmpty(request.PostalCode))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Postal code"
                };
            }

            if (string.IsNullOrEmpty(request.State))
            {
                return new ApiResponse
                {
                    Message = "Please supply your State"
                };
            }

            if (string.IsNullOrEmpty(request.MailingCity))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing City"
                };
            }

            if (string.IsNullOrEmpty(request.MailingCountry))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing Country"
                };
            }

            if (string.IsNullOrEmpty(request.MailingAddress1))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing Address1"
                };
            }

            if (string.IsNullOrEmpty(request.MailingAddress2))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing Address2"
                };
            }

            if (string.IsNullOrEmpty(request.MailingPostalCode))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing Postal code"
                };
            }

            if (string.IsNullOrEmpty(request.MailingState))
            {
                return new ApiResponse
                {
                    Message = "Please supply your mailing State"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdatePersonal(UpdatePersonalRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid UserId"
                };
            }

            if (string.IsNullOrEmpty(request.BankStatement))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Bank statement"
                };
            }

            if (string.IsNullOrEmpty(request.Citizenship))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Citizenship"
                };
            }

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse
                {
                    Message = " Invalid Phone number"
                };
            }

            if (string.IsNullOrEmpty(request.TaxDocument))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Tax Document"
                };
            }

            if (string.IsNullOrEmpty(request.TaxNumber))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Tax number"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateAddress(UpdateAddressRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid userId"
                };
            }

            // if (BusinessId == 0)
            // {
            //     return new ApiResponse
            //     {
            //         Message = " Invalid BusinessId"
            //     };
            // }

            if (string.IsNullOrEmpty(request.City))
            {
                return new ApiResponse
                {
                    Message = "Please supply your City"
                };
            }

            if (string.IsNullOrEmpty(request.Address1))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Address1"
                };
            }

            if (string.IsNullOrEmpty(request.Address2))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Address2"
                };
            }

            if (string.IsNullOrEmpty(request.PostalCode))
            {
                return new ApiResponse
                {
                    Message = "Please supply your postal code"
                };
            }

            if (string.IsNullOrEmpty(request.State))
            {
                return new ApiResponse
                {
                    Message = "Please supply your state"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateEmployment(UpdateEmploymentRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid userId"
                };
            }

            // if (BusinessId == 0)
            // {
            //     return new ApiResponse
            //     {
            //         Message = " Invalid BusinessId"
            //     };
            // }

            if (string.IsNullOrEmpty(request.Employer))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Employer"
                };
            }

            if (string.IsNullOrEmpty(request.EmployerCountry))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Employer Country"
                };
            }

            if (string.IsNullOrEmpty(request.EmployerState))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Employer State"
                };
            }

            if (string.IsNullOrEmpty(request.EmploymentStatus))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Employment Status"
                };
            }

            if (string.IsNullOrEmpty(request.Occupation))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Occupation"
                };
            }

            if (string.IsNullOrEmpty(request.OccupationDescription))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Occupation Description"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateIdentity(UpdateIdentityRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid userId"
                };
            }

            if (string.IsNullOrEmpty(request.IdentityDocumentFront))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Identity document"
                };
            }

            if (!string.Equals(request.IdentityType, Identifications.Passport, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(request.IdentityDocumentBack))
            {
                return new ApiResponse
                {
                    Message = "Please supply your back Identity document"
                };
            }

            if (request.IdentityExpiryDate==DateTime.MinValue)
            {
                return new ApiResponse
                {
                    Message = "Please supply Expiry Date of your document"
                };
            }

            if (request.IdentityIssueDate == DateTime.MinValue)
            {
                return new ApiResponse
                {
                    Message = "Please supply the Issue Date of your document"
                };
            }

            if (string.IsNullOrEmpty(request.IdentityNumber))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Identity number"
                };
            }

            if (string.IsNullOrEmpty(request.IdentityType))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Identity type"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateIncome(UpdateIncomeRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid userId"
                };
            }

            // if (BusinessId == 0)
            // {
            //     return new ApiResponse
            //     {
            //         Message = " Invalid BusinessId"
            //     };
            // }

            if (string.IsNullOrEmpty(request.AnnualIncome))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Annual Income"
                };
            }

            if (string.IsNullOrEmpty(request.IncomeCountry))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Income Country"
                };
            }

            if (string.IsNullOrEmpty(request.IncomeSource))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Income Source"
                };
            }

            if (string.IsNullOrEmpty(request.IncomeState))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Income State"
                };
            }

            if (string.IsNullOrEmpty(request.OwnershipPercentage))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Ownership Percentage"
                };
            }

            //if (string.IsNullOrEmpty(request.Role))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Invalid Role "
            //    };
            //}

            if (string.IsNullOrEmpty(request.SourceOfFunds))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Source of Funds"
                };
            }

            //if (string.IsNullOrEmpty(request.Title))
            //{
            //    return new ApiResponse
            //    {
            //        Message = " Invalid Title"
            //    };
            //}

            if (string.IsNullOrEmpty(request.WealthSource))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Wealth source"
                };
            }

            if (string.IsNullOrEmpty(request.WealthSourceDescription))
            {
                return new ApiResponse
                {
                    Message = "Please supply your Wealth source description"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse UpdateAuthorizationPin(UpdateAuthorizationPinRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly provide a valid Id"
                };
            }

            if (string.IsNullOrEmpty(request.CurrentPin))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply a valid pin"
                };
            }

            if (string.IsNullOrEmpty(request.Pin))
            {
                return new ApiResponse
                {
                    Message = " Kindly supply a valid pin"
                };
            }

            if (string.IsNullOrEmpty(request.PinConfirmation))
            {
                return new ApiResponse
                {
                    Message = " Kindly supply a valid pin"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                return new ApiResponse
                {
                    Message = "Password must have minimum of 8 characters"
                };
            }
            if (!password.Any(char.IsUpper))
            {
                return new ApiResponse
                {
                    Message = "Password must have minimum of 8 characters"
                };
            }
            if (!password.Any(char.IsDigit))
            {
                return new ApiResponse
                {
                    Message = "Password must contain at least one digit"
                };
            }
            if (Regex.Match(password, @"^[a-zA-Z0-9 ]*$").Success)
            {
                return new ApiResponse
                {
                    Message = "Password must contain at least one special character"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse CompleteOnboarding(CompleteOnboardingRequest request)
        {
            if (request.AccountTypeId == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Account Type"
                };
            }

            if (request.PurposeOfAccount.Length == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your purpose of account"
                };
            }

            if (request.OccupationId == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your occupation"
                };
            }

            if (string.IsNullOrEmpty(request.IdentificationType))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your means of identification"
                };
            }

            if (string.IsNullOrEmpty(request.JobBrief))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply short brief about yourself"
                };
            }
            if (request.CountryCode == Countries.Nigeria && string.IsNullOrEmpty(request.IndividualBVN))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply BVN"
                };
            }
            if (request.CountryCode == Countries.Nigeria && request.IndividualBVN != null && (request.IndividualBVN.Length < 11 || request.IndividualBVN.Length > 11))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid BVN"
                };
            }

            if (request.IndividualBVN != null && !IsBVNValid(request.IndividualBVN))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid BVN"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessType))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply business type"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessName))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Name"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.NatureOfBusinessId == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your nature of the Business"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.JobBrief))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the Business description"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.PostalCode))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply Business Post code"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RegisteredAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Registered Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.OperatingAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Operating Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && (request.DateOfIncorporation == null || request.DateOfIncorporation == DateTime.MinValue))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply Date of Incorporation"
                };
            }

            if (request.CountryCode == Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && request.RegistrationBodyId == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business RegistrationBody"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && string.IsNullOrEmpty(request.BusinessLicenseNumber))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business License Number"
                };
            }
            if (request.CountryCode == Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.CompanyNumber))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Number"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionValue))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Transaction Value"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionVolume))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Business Transaction Volume"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your Date of birth"
                };
            }
            if (request.CountryCode != Countries.Nigeria && request.DestinationOfFunds is null)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply destination of funds"
                };
            }
            if (request.CountryCode != Countries.Nigeria && request.DestinationOfFunds is not null && request.DestinationOfFunds.Length > 3)
            {
                return new ApiResponse
                {
                    Message = "You have exceeded the max limit for destination"
                };
            }
            if (request.CountryCode == Countries.Nigeria && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.DirectorName))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply Name of Director"
                };
            }
            if (request.CountryCode == Countries.Nigeria && request.AccountTypeId == OnboardingAccountTypes.Business && !string.IsNullOrEmpty(request.DirectorBVN) && !IsBVNValid(request.DirectorBVN))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid BVN for Director"
                };
            }
            if (request.CountryCode != Countries.United_Kingdom && !string.IsNullOrEmpty(request.FaceBook) && !IsURLValid(request.FaceBook))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid Facebook URL"
                };
            }
            if (request.CountryCode != Countries.United_Kingdom && !string.IsNullOrEmpty(request.Instagram) && !IsURLValid(request.Instagram))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid Instagram URL"
                };
            }
            if (request.CountryCode != Countries.United_Kingdom && !string.IsNullOrEmpty(request.Twitter) && !IsURLValid(request.Twitter))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid Twitter URL"
                };
            }
            if (request.CountryCode != Countries.United_Kingdom && !string.IsNullOrEmpty(request.Website) && !IsURLValid(request.Website))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid Website URL"
                };
            }
            //if (request.CountryCode != Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.IdentificationFront))
            //{
            //    return new AppResult
            //    {
            //        Returned
            //        ResponseMessage = "Kindly supply front of your Identification"
            //    };
            //}
            //if (request.CountryCode != Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.IdentificationBack))
            //{
            //    return new AppResult
            //    {
            //        Returned
            //        ResponseMessage = "Kindly supply back of your Identification"
            //    };
            //}
            //if (request.CountryCode != Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.CertificateFront))
            //{
            //    return new AppResult
            //    {
            //        Returned
            //        ResponseMessage = "Kindly supply front of your certificate"
            //    };
            //}
            //if (request.CountryCode != Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.CertificateBack))
            //{
            //    return new AppResult
            //    {
            //        Returned
            //        ResponseMessage = "Kindly supply back of your certificate"
            //    };
            //}
            //if (request.CountryCode != Countries.United_Kingdom && request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.UtilityBill))
            //{
            //    return new AppResult
            //    {
            //        Returned
            //        ResponseMessage = "Kindly supply your address verification document"
            //    };
            //}
            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse CompleteKyc(CompleteKycRequest request)
        {
            if (request.AccountTypeId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Account Type"
                };
            }

            if (request.PurposeOfAccount.Length == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your purpose of account"
                };
            }

            if (request.OccupationId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your occupation"
                };
            }

            if (string.IsNullOrEmpty(request.IdentificationType))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your means of identification"
                };
            }

            if (string.IsNullOrEmpty(request.JobBrief))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply short brief about yourself"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessType))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply business type"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Name"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.NatureOfBusinessId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your nature of the Business"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.JobBrief))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Business description"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.PostalCode))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Business Post code"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RegisteredAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Registered Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.OperatingAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Operating Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && (request.DateOfIncorporation == null || request.DateOfIncorporation == DateTime.MinValue))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Date of Incorporation"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && request.RegistrationBodyId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business RegistrationBody"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && string.IsNullOrEmpty(request.CompanyNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business License Number"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionValue))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Value"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionVolume))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Volume"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Date of birth"
                };
            }
            if (request.DestinationOfFunds.Length == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply destination of funds"
                };
            }
            if (request.DestinationOfFunds.Length > 3)
            {
                return new ApiResponse
                {

                    Message = "You have exceeded the max limit for destination"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse VerifyPhoneNumber(PhoneVerificationRequest request, string countryCode)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your phone Number"
                };
            }
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply FirstName"
                };
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your LastName"
                };
            }

            if (request.PhoneNumber.Length < 11 || request.PhoneNumber.Length > 11)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }

            if (!IsMobileNumberValid(request.PhoneNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }

            if (countryCode == Countries.Nigeria && string.IsNullOrEmpty(request.Bvn))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply BVN"
                };
            }
            if (countryCode == Countries.Nigeria && request.Bvn != null && (request.Bvn.Length < 11 || request.Bvn.Length > 11))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            if (request.Bvn != null && !IsBVNValid(request.Bvn))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            if (request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Date of birth"
                };
            }

            long dateDiff = DateDiff(DateInterval.Year, request.DateOfBirth, DateTime.Today);
            if (dateDiff < 18)
            {
                return new ApiResponse
                {

                    Message = "Date Of Birth must be greater than 18"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse CompleteSignUp(CompleteSignUpRequest request)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your password"
                };
            }
            if (string.IsNullOrEmpty(request.PasswordConfirmation))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the password confirmation"
                };
            }

            if (request.Password != request.PasswordConfirmation)
            {
                return new ApiResponse
                {
                    Message = "Password and password confirmation must match"
                };
            }

            if (!IsPasswordValid(request.Password))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid password"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }


        public long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = ts = date2 - date1;

            return interval switch
            {
                DateInterval.Year => date2.Year - date1.Year,
                DateInterval.Month => (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year)),
                DateInterval.Weekday => Fix(ts.TotalDays) / 7,
                DateInterval.Day => Fix(ts.TotalDays),
                DateInterval.Hour => Fix(ts.TotalHours),
                DateInterval.Minute => Fix(ts.TotalMinutes),
                _ => Fix(ts.TotalSeconds),
            };
        }

        public long MinuteDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = ts = date2 - date1;

            return interval switch
            {
                //DateInterval.Year => date2.Year - date1.Year,
                //DateInterval.Month => (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year)),
                //DateInterval.Weekday => Fix(ts.TotalDays) / 7,
                //DateInterval.Day => Fix(ts.TotalDays),
                //DateInterval.Hour => Fix(ts.TotalHours),
                DateInterval.Minute => Fix(ts.Minutes),
                _ => Fix(ts.TotalSeconds),
            };
        }

        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        public ApiResponse FinalizeOnboarding(FinalizeOnboardingRequest request)
        {

            if (string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your password"
                };
            }
            if (string.IsNullOrEmpty(request.PasswordConfirmation))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply password confirmation"
                };
            }

            if (request.PasswordConfirmation != request.Password)
            {
                return new ApiResponse
                {
                    Message = "Password must match password confirmation"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }
        public ApiResponse ConfirmPhoneNumber(ConfirmPhoneNoRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your phone Number"
                };
            }
            if (string.IsNullOrEmpty(request.ResidentialAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Residential Address"
                };
            }

            if (string.IsNullOrEmpty(request.PostalCode))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Postalcode"
                };
            }

            if (request.PhoneNumber.Length < 10 || request.PhoneNumber.Length > 10)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }

            if (!IsMobileNumberValid(request.PhoneNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your password"
                };
            }
            if (string.IsNullOrEmpty(request.PasswordConfirmation))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply password confirmation"
                };
            }

            if (request.PasswordConfirmation != request.Password)
            {
                return new ApiResponse
                {
                    Message = "Password must match password confirmation"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse SignIn(SignInRequest request)
        {
            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your email"
                };
            }

            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid emailAddress"
                };
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your password"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse InitiateSignUp(InitiateSignUpRequest request)
        {
            if (string.IsNullOrEmpty(request.AccountType))
            {
                return new ApiResponse
                {
                    Message = "Kindly select valid account type"
                };
            }

            if (string.IsNullOrEmpty(request.FirstName))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the first name"
                };
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the last name"
                };
            }

            if (string.Equals(request.AccountType, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the name of your business"
                };
            }

            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your emailAddress"
                };
            }

            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid emailAddress"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse ConfirmEmailAddress(ConfirmEmailAddressRequest request)
        {
            if (string.IsNullOrEmpty(request.CountryCode))
            {
                return new ApiResponse
                {

                    Message = "Kindly select valid Country"
                };
            }

            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your emailAddress"
                };
            }

            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid emailAddress"
                };
            }

            if (request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your date of birth"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse BusinessOnboarding(BusinessOnboardingRequest request)
        {
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Name"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RegisteredAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Registered Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.OperatingAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Operating Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && (request.DateOfIncorporation == null || request.DateOfIncorporation == DateTime.MinValue))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Date of Incorporation"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.NatureOfBusinessId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your nature of the Business"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.RegistrationBodyId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business RegistrationBody"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.LicenseIDNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business License Number"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.TransactionValue == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Value"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.TransactionVolume == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Volume"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RepresentativeName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Representative"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RepresentativeAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Address of Business Representative"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RepresentativeDOB))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Date Of Birth for Business Representative"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.RepresentativeIdType == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Means of Identification for Representative"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse ValidateInvitation(InvitationRequest request, long businessId)
        {
            if (request.RoleId == 0)
            {
                return new ApiResponse
                {
                    Message = "Please supply the role"
                };
            }
            if (businessId == 0)
            {
                return new ApiResponse
                {
                    Message = "Please supply the Business"
                };
            }
            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Please supply the email address"
                };
            }

            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid email address"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse ValidateUser(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Role))
            {
                return new ApiResponse
                {
                    Message = "Please supply the role"
                };
            }
            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Please supply the email address"
                };
            }

            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply valid email address"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse Transfer(TransferRequest request)
        {
            //if (request.Actor == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the sender details"
            //    };
            //}
            //if (request.AccountId == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the Account details"
            //    };
            //}
            if (string.IsNullOrEmpty(request.TransactionPin))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Transaction Pin"
                };
            }
            if (string.IsNullOrEmpty(request.Currency))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Currency"
                };
            }
            //if (request.BeneficiaryId == 0 && string.IsNullOrEmpty(request.BeneficiaryCountry))
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the receiver Country code"
            //    };
            //}

            if (request.Amount == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the amount"
                };
            }

            if (request.Amount < 0)
            {
                return new ApiResponse
                {
                    Message = "Amount must be greater than zero"
                };
            }
            //if (request.BeneficiaryId == 0 && request.PaymentType == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Please supply payment type"
            //    };
            //}
            //if (request.BankCountry.ToLower() != Countries.Nigeria.ToLower() && request.BeneficiaryId == 0 && request.BeneficiaryEntityTypeId == 0)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Please supply Entity type"
            //    };
            //}
            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse InternalTransfer(InternalTransferRequest request)
        {
            //if (request.Actor == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the sender details"
            //    };
            //}
            //if (request.AccountId == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the Account details"
            //    };
            //}
            if (string.IsNullOrEmpty(request.TransactionPin))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Transaction Pin"
                };
            }
            if (string.IsNullOrEmpty(request.Currency))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Currency"
                };
            }

            //if (string.IsNullOrEmpty(request.AccountNumber))
            //{
            //    return new AppResult
            //    {
            //        Message = "Please supply Account Number"
            //    };
            //}

            if (string.IsNullOrEmpty(request.AccountNumber) && request.BeneficiaryId == 0)
            {
                return new ApiResponse
                {
                    Message = "Please supply the beneficiary"
                };
            }

            if (request.Amount == 0)
            {
                return new ApiResponse
                {
                    Message = "Kindly supply the amount"
                };
            }

            if (request.Amount < 0)
            {
                return new ApiResponse
                {
                    Message = "Amount must be greater than zero"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse GetRecieverRequirement(GetBeneficiaryRequirement beneficiaryRequirement)
        {
            if (string.IsNullOrEmpty(beneficiaryRequirement.ReceiverCountryCode))
            {
                return new ApiResponse
                {

                    Message = "kindly supply the receiver country"
                };
            }

            if (beneficiaryRequirement.PaymentTypeId == 0)
            {
                return new ApiResponse
                {

                    Message = "kindly supply the payment type"
                };
            }

            if (beneficiaryRequirement.ReceiverEntityTypeId == 0)
            {
                return new ApiResponse
                {

                    Message = "kindly supply the Entity type"
                };
            }

            if (beneficiaryRequirement.ReceiverCountryCode.ToUpper() != Countries.United_Kingdom && beneficiaryRequirement.PaymentTypeId == PaymentTypes.Local)
            {
                return new ApiResponse
                {

                    Message = "kindly supply the valid payment type"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse CreateCustomerAccount(CompleteOnboardingRequest request)
        {
            if (request.AccountTypeId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Account Type"
                };
            }

            if (request.PurposeOfAccount.Length == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your purpose of account"
                };
            }

            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Email Address"
                };
            }
            if (!IsEmailValid(request.EmailAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid emailAddress"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessType))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply business type"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Individual && string.IsNullOrEmpty(request.AccountName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Account Name"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.BusinessName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Name"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.NatureOfBusinessId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your nature of the Business"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.JobBrief))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Business description"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.RegisteredAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Registered Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.OperatingAddress))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Operating Address"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && (request.DateOfIncorporation == null || request.DateOfIncorporation == DateTime.MinValue))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Date of Incorporation"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && request.IsBusinessRegulated && request.RegistrationBodyId == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business RegistrationBody"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.IsBusinessRegulated && string.IsNullOrEmpty(request.BusinessLicenseNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business License Number"
                };
            }
            if (request.AccountTypeId == OnboardingAccountTypes.Business && string.IsNullOrEmpty(request.CompanyNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Number"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionValue))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Value"
                };
            }
            if (string.IsNullOrEmpty(request.TransactionVolume))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Business Transaction Volume"
                };
            }

            if (request.AccountTypeId == OnboardingAccountTypes.Business && request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your Date of Birth"
                };
            }
            if (request.DestinationOfFunds is null)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply destination of funds"
                };
            }
            if (request.DestinationOfFunds.Length > 3)
            {
                return new ApiResponse
                {

                    Message = "You have exceeded the max limit for destination"
                };
            }
            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse ReceiveMoney(ReceiveMoneyRequest request)
        {
            //if (request.Actor == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the sender details"
            //    };
            //}
            //if (request.AccountId == 0)
            //{
            //    return new ApiResponse
            //    {

            //        Message = "Kindly supply the Account details"
            //    };
            //}

            if (string.IsNullOrEmpty(request.Currency))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Currency"
                };
            }
            if (string.IsNullOrEmpty(request.EmailOrPhoneNo))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the Beneficiary"
                };
            }

            if (request.Amount == 0)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply the amount"
                };
            }

            if (request.Amount < 0)
            {
                return new ApiResponse
                {

                    Message = "Amount must be greater than zero"
                };
            }
            if (request.EmailOrPhoneNo.Contains("@") && !IsEmailValid(request.EmailOrPhoneNo))
            {
                return new ApiResponse
                {

                    Message = "Please supply valid email"
                };
            }
            if (request.PaymentTypeId == 0)
            {
                return new ApiResponse
                {

                    Message = "Please supply payment type"
                };
            }
            if (!request.EmailOrPhoneNo.Contains("@") && request.EmailOrPhoneNo.Length < 12)
            {
                return new ApiResponse
                {

                    Message = "Please supply Entity type"
                };
            }
            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse SendSms(SendSmsRequest request)
        {
            if (string.IsNullOrEmpty(request.CountryCode))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your CountryCode"
                };
            }
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your phone Number"
                };
            }
            if (string.IsNullOrEmpty(request.Message))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your message"
                };
            }

            if (request.PhoneNumber.Length < 11 || request.PhoneNumber.Length > 11)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }

            if (!IsMobileNumberValid(request.PhoneNumber.Substring(1)))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid phone Number"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }


        public ApiResponse BVNLookUp(string bvn)
        {
            if (string.IsNullOrEmpty(bvn))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply BVN"
                };
            }
            if (bvn.Length < 11 || bvn.Length > 11)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            if (!IsBVNValid(bvn))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse DirectorBVNLookUp(ValidateDirectorBvnRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply FirstName"
                };
            }
            if (string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply LastName"
                };
            }
            if (request.DateOfBirth == DateTime.MinValue)
            {
                return new ApiResponse
                {

                    Message = "Kindly supply Date Of Birth"
                };
            }
            if (string.IsNullOrEmpty(request.BVN))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply BVN"
                };
            }
            if (request.BVN != null && (request.BVN.Length < 11 || request.BVN.Length > 11))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            if (!IsBVNValid(request.BVN))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply valid BVN"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse SetupPin(PinRequest request)
        {
            if (string.IsNullOrEmpty(request.Pin))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply your pin"
                };
            }

            if (string.IsNullOrEmpty(request.PinConfirmation))
            {
                return new ApiResponse
                {

                    Message = "Kindly supply pin confirmation"
                };
            }

            if (request.PinConfirmation != request.Pin)
            {
                return new ApiResponse
                {
                    Message = "pin must match pin confirmation"
                };
            }

            if (request.Pin.Length != 6)
            {
                return new ApiResponse
                {

                    Message = "invalid length of pin"
                };
            }

            if (!IsPinValid(request.Pin))
            {
                return new ApiResponse
                {

                    Message = "Please supply only digit"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse UpdatePin(PinRequest request)
        {
            if (string.IsNullOrEmpty(request.OldPin))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply your previous pin"
                };
            }

            if (string.IsNullOrEmpty(request.Pin))
            {
                return new ApiResponse
                {
                    Message = "Kindly supply new pin"
                };
            }

            if (request.OldPin == request.Pin)
            {
                return new ApiResponse
                {
                    Message = "pin must not match old pin confirmation"
                };
            }

            if (request.Pin.Length != 4)
            {
                return new ApiResponse
                {
                    Message = "invalid length of pin"
                };
            }

            if (!IsPinValid(request.Pin))
            {
                return new ApiResponse
                {
                    Message = "Please supply only digit"
                };
            }

            return new ApiResponse
            {
                Success = true
            };
        }

        public ApiResponse Trade(TradeCryptoRequest request)
        {
            if (string.IsNullOrEmpty(request.Asset))
            {
                return new ApiResponse
                {
                    Message = "Please supply the asset"
                };
            }

            if (string.IsNullOrEmpty(request.Network))
            {
                return new ApiResponse
                {
                    Message = "Please supply the network"
                };
            }

            if (string.IsNullOrEmpty(request.TransactionPin))
            {
                return new ApiResponse
                {
                    Message = "Please supply the transaction pin"
                };
            }

            if (request.FromAmount == 0)
            {
                return new ApiResponse
                {
                    Message = "Please supply the source amount"
                };
            }

            if (request.FromAmount == 0)
            {
                return new ApiResponse
                {
                    Message = "Please supply the target amount"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public ApiResponse TradeFx(TradeFxRequest request, long userId)
        {
            if (userId == 0)
            {
                return new ApiResponse
                {
                    Message = "Invalid user ID"
                };
            }

            //if (string.IsNullOrEmpty(request.FromCurrency))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Please supply base currency"
            //    };
            //}

            //if (string.IsNullOrEmpty(request.ToCurrency))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Please supply the target currency"
            //    };
            //}

            if (string.IsNullOrEmpty(request.TransactionPin))
            {
                return new ApiResponse
                {
                    Message = "Please supply the transaction pin"
                };
            }

            //if (request.FromAmount == 0)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Please supply the source amount"
            //    };
            //}

            //if (request.ToAmount == 0)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Please supply the target amount"
            //    };
            //}

            return new ApiResponse { Message = "Successful", Success = true };
        }
    }
}
