using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Notification;

namespace GabTrans.Application.Services
{
    public class RepresentativeService(ILogService logManager, IAuditRepository auditRepository, IBusinessRepository businessRepository, IEmailNotificationService emailService, ISignUpRepository signUpRepository, IKycRepository kycRepository, IUserRepository userRepository) : IRepresentativeService
    {
        private readonly ILogService _logService = logManager;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<bool> CreateAsync(long userId, long businessId, string companyNumber)
        {
            var officers = new List<Officers>();

            //try
            //{
            //    var kyc = await _signUpRepository.GetKycByUserIdAsync(userId);
            //    if (kyc == null) return false;

            //    var user = await _signUpRepository.GetUserIdAsync(userId);
            //    if (user == null) return false;

            //    //switch (kyc.CountryCode.ToUpper())
            //    //{
            //    //    case Countries.United_Kingdom:
            //    //        officers = await GetUKPeoplesAsync(companyNumber);
            //    //        break;
            //    //    default:
            //    //        break;
            //    //}

            //    if (officers == null || officers.Count == 0) return false;

            //    return await _directorRepository.InsertAsync(new SaveOfficers { BusinessId = businessId, Officers = officers, UserId = userId });
            //}
            //catch (Exception ex)
            //{
            //    _logService.LogError("DirectorService", "Create", ex);
            //}
            return false;
        }


        public async Task<List<RepresentativeModel>> GetListAsync(long userId)
        {
            return new List<RepresentativeModel>(); // await _kycRepository.DetailsByUserAsync(userId);
        }

        public async Task<ApiResponse> UpdateAddressAsync(UpdateAddressRequest request, long userId)
        {
            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }

            kyc.Address1 = request.Address1;
            kyc.Address2 = request.Address2;
            kyc.City = request.City;
            kyc.ResidentialState = request.State;
            kyc.Country = request.Country;
            kyc.PostalCode = request.PostalCode;

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public async Task<ApiResponse> UpdateEmploymentAsync(UpdateEmploymentRequest request, long userId)
        {
            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }

            kyc.Occupation = request.Occupation;
            kyc.OccupationDescription = request.OccupationDescription;
            kyc.EmploymentStatus = request.EmploymentStatus;
            kyc.Employer = request.Employer;
            kyc.EmployerState = request.EmployerState;
            kyc.EmployerCountry = request.EmployerCountry;

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public async Task<ApiResponse> UpdateIdentityAsync(UpdateIdentityRequest request, long userId)
        {
            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }

            kyc.IdentityNumber = request.IdentityNumber;
            kyc.IdentityType = request.IdentityType;
            kyc.IdentityIssueDate = request.IdentityIssueDate;
            kyc.IdentityExpiryDate = request.IdentityExpiryDate;

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public async Task<ApiResponse> UpdateIncomeAsync(UpdateIncomeRequest request, long userId)
        {
            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }


            kyc.IncomeSource = request.IncomeSource;
            kyc.IncomeState = request.IncomeState;
            kyc.IncomeCountry = request.IncomeCountry;
            kyc.WealthSource = request.WealthSource;
            kyc.WealthSourceDescription = request.WealthSourceDescription;
            kyc.AnnualIncome = request.AnnualIncome;

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public async Task<ApiResponse> UpdatePersonalAsync(UpdatePersonalRequest request, long userId)
        {
            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }

            user.PhoneNumber = request.PhoneNumber;
            kyc.DateOfBirth = request.DateOfBirth;
            kyc.TaxNumber = request.TaxNumber;
            kyc.Citizenship = request.Citizenship;
            kyc.TaxDocument = request.TaxDocument;
            kyc.BankStatement = request.BankStatement;

            await _userRepository.UpdateAsync(user);

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }

        public async Task<ApiResponse> UpdateCountryOfOperationAsync(UpdateCountryOperationRequest request, long userId)
        {
            var detail = await _kycRepository.DetailsByUserAsync(userId);
            if (detail == null)
            {
                return new ApiResponse
                {
                    Message = "Details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Business details not found"
                };
            }

            //kyc.Country = request.Country;

            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update details"
                };
            }

            return new ApiResponse { Message = "Successful", Success = true };
        }
    }
}
