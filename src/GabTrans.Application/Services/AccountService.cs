using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using OfficeOpenXml.Drawing.Slicer.Style;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace GabTrans.Application.Services
{
    public class AccountService(ILogService logService, IFileService fileService, IAuditService auditService, IEmailNotificationService emailService, IKycRepository kycRepository, IUserRepository userRepository, IPasswordService passwordService, ILimitRepository limitRepository, IInfinitusService infinitusService, IWalletRepository walletRepository, ISignUpRepository signUpRepository, IValidationService validationService, ICountryRepository countryRepository, IAccountRepository accountRepository, IBusinessRepository businessRepository, IAccountRequestRepository accountRequestRepository) : IAccountService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly IAuditService _auditService = auditService;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ILimitRepository _limitRepository = limitRepository;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IValidationService _validationService = validationService;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IAccountRequestRepository _accountRequestRepository = accountRequestRepository;

        //public async Task OpenAccountAsync()
        //{
        //    try
        //    {

        //        var accounts = await _accountRepository.GetAsync(AccountStatuses.Pending);
        //        foreach (var account in accounts)
        //        {
        //            if (string.IsNullOrEmpty(account.Uuid))
        //            {
        //                _logService.LogInfo(nameof(AccountService), nameof(OpenAccountAsync), $"UUID details not found for user ID {account.UserId}");
        //                continue;
        //            }

        //            var kyc = await _kycRepository.DetailsByUserAsync(account.UserId);
        //            if (kyc is null)
        //            {
        //                _logService.LogInfo(nameof(AccountService), nameof(OpenAccountAsync), $"Kyc details not found for user ID {kyc.UserId}");
        //                continue;
        //            }

        //            var country = await _countryRepository.GetCountryDetailsAsync(kyc.Country);
        //            if (country is null)
        //            {
        //                _logService.LogInfo(nameof(AccountService), nameof(OpenAccountAsync), $"country details not found for user ID {kyc.UserId}");
        //                continue;
        //            }

        //            var user = await _userRepository.GetDetailsByUserIdAsync(kyc.UserId);
        //            if (user is null)
        //            {
        //                _logService.LogInfo(nameof(AccountService), nameof(OpenAccountAsync), $"User details not found for user ID {kyc.UserId}");
        //                continue;
        //            }

        //            var response = await _infinitusService.CreateGlobalAccountAsync(account, country.Currency, kyc.Country);
        //            if (!response.Success)
        //            {
        //                _logService.LogInfo(nameof(AccountService), nameof(OpenAccountAsync), $"country details not found for user ID {kyc.UserId}");
        //                continue;
        //            }

        //            await _emailService.AccountOpeningAsync(user.EmailAddress, user.FirstName);

        //            account.Status = AccountStatuses.Active;
        //            await _accountRepository.UpdateAsync(account);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError(nameof(AccountService), nameof(OpenAccountAsync), ex);
        //    }
        //}

        public async Task<ApiResponse> OpenAccountAsync(long userId)
        {
            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the user"
                };
            }

            var country = await _countryRepository.GetCountryDetailsAsync(kyc.Country);
            if (country is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the country"
                };
            }

            var response = await _infinitusService.GetAccountRequestAsync(kyc);
            if (!response.Success) return response;

            var accountResponse = (InfinitusAccountResponse)response.Data;

            var user = await _userRepository.GetDetailsByUserIdAsync(kyc.UserId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch user details"
                };
            }

            var business = await _businessRepository.GetByUserAsync(kyc.UserId);
            if (business is null && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch business details"
                };
            }

            var account = new Account
            {
                UserId = kyc.UserId,
                Name = string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) ? business.Name : $"{user.FirstName} {user.LastName}",
                CreatedAt = DateTime.Now,
                Type = kyc.Type,
                Status = AccountStatuses.Active,
                Uuid = accountResponse.id,
                Provider = accountResponse.provider
            };
            long accountId = await _accountRepository.InsertAsync(account);
            if (accountId == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to create account"
                };
            }

            kyc.Status = KycStatuses.Passed;
            await _kycRepository.UpdateAsync(kyc);

            response = await _infinitusService.CreateGlobalAccountAsync(account, country.Currency, kyc.Country);
            if (!response.Success) return response;

            await _emailService.AccountOpeningAsync(user.EmailAddress, user.FirstName,country.Currency);

            return response;
        }

        public async Task<ApiResponse> DetailsAsync(long accountId)
        {
            var details = new AccountDetailsModel();

            var account = await _accountRepository.GetAccountDetailsAsync(accountId);

            if (account == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Account not found"
                };
            }

            details.Id = account.Id;
            details.Name = account.Name;
            details.EmailAddress = account.EmailAddress;
            details.Type = account.Type;
            details.CreatedAt = account.CreatedAt;

            details.PersonalInformation = account.PersonalInformation;
            details.EmploymentInformation = account.EmploymentInformation;
            details.IdentityDocument = account.IdentityDocument;

            return new ApiResponse { Success = true, Message = "Successful", Data = details };
        }

        public Task ProcessAsync()
        {
            throw new NotImplementedException();
        }
    }
}

