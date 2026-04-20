using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Newtonsoft.Json;

namespace GabTrans.Application.Services
{
    public class ComplianceService(ILogService logService, IEmailNotificationService emailService, IKycRepository kycRepository, IBusinessService businessService, IUserRepository userRepository, IValidationService validationService, IAccountRepository accountRepository, ISignUpRepository signUpRepository, IInfinitusService infinitusService, IBusinessRepository businessRepository, IBusinessTeamRepository businessTeamRepository, IDepositRepository transferRepository, ICountryRepository countryRepository, IAzureFileClientIntegration azureFileClientIntegration) : IComplianceService
    {
        private readonly ILogService _logService = logService;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly IBusinessService _businessService = businessService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IValidationService _validationService = validationService;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IBusinessTeamRepository _businessTeamRepository = businessTeamRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IDepositRepository _transferRepository = transferRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IAzureFileClientIntegration _azureFileClientIntegration = azureFileClientIntegration;

        public async Task CreateAsync()
        {
            //var users = await _userRepository.GetAsync();
            //foreach (var user in users)
            //{
            //    string? businessName = string.Empty;
            //    var kyc = await _kycRepository.DetailsByUserAsync(user.Id);
            //    if (kyc is null) continue;

            //    if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
            //    {
            //        var business = await _businessRepository.GetByUserAsync(user.Id);
            //        if (business is null) continue;
            //        businessName = business.Name;
            //    }
            //    var response = await _infinitusService.CreateClientAsync(user, kyc.Type);
            //    if (!response.Success) continue;
            //}

            var kyc = await _kycRepository.DetailsByUserAsync(1);
            if (kyc is null)
            {

            }
            //var response = await _infinitusService.DocumentClientAsync(kyc);
            //if (!response.Success)
            //{

            //}

            //var business=await _businessRepository.DetailsByIdAsync(4);

            //var response = await _infinitusService.UpdateBusinessClientAsync(business,4);
            //if (!response.Success)
            //{

            //}
            var res = await SubmitApplicationAsync(kyc);
        }

        public async Task<ApiResponse> CreateClientAsync(User user, string type, string businessName)
        {
            return await _infinitusService.CreateClientAsync(user, type);
        }

        public async Task<ApiResponse> SubmitApplicationAsync(Kyc kyc)
        {
            ApiResponse? response;

            var user = await _userRepository.GetDetailsByUserIdAsync(kyc.UserId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "No user details found"
                };
            }

            var business = await _businessRepository.GetByUserAsync(kyc.UserId);
            if (business is null && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "No business details found"
                };
            }

            response = await _infinitusService.CreateClientAsync(user, kyc.Type);
            if (!response.Success) return response;

            if (string.Equals(kyc.Type, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase))
            {
                response = await _infinitusService.UpdatePersonalClientAsync(user);
                if (!response.Success) return response;
                //Upload documents
                response = await _infinitusService.DocumentClientAsync(kyc);
                if (!response.Success) return response;
            }

            if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
            {
                response = await _infinitusService.UpdateBusinessClientAsync(business, kyc.UserId);
                if (!response.Success) return response;

                response = await _infinitusService.CreateRepresentativeAsync(user);
                if (!response.Success) return response;

                //Upload documents
                response = await _infinitusService.DocumentRepresentativeAsync(kyc);
                if (!response.Success) return response;

                response = await _infinitusService.DocumentBusinessClientAsync(kyc);
                if (!response.Success) return response;
            }

            List<string> providers = [.. InfinitusPayProviders.Fortress.Split(',')];

            return await _infinitusService.SubmitClientAsync(kyc, providers);
        }
    }
}
