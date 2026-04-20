using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class AccountRequestService(ILogService logService, IFileService fileService, IAuditService auditService, IEmailNotificationService emailService, IKycRepository kycRepository, IUserRepository userRepository, IPasswordService passwordService, ILimitRepository limitRepository, IInfinitusService infinitusService, IWalletRepository walletRepository, ISignUpRepository signUpRepository, IValidationService validationService, ICountryRepository countryRepository, IAccountRepository accountRepository, IGlobusBankService globusBankService, IBusinessRepository businessRepository, IAccountRequestRepository accountRequestRepository) : IAccountRequestService
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
        private readonly IGlobusBankService _globusBankService = globusBankService;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IAccountRequestRepository _accountRequestRepository = accountRequestRepository;

        public async Task ProcessAsync()
        {
            try
            {
                var accountRequests = await _accountRequestRepository.GetAsync(AccountStatuses.Approved);
                foreach (var accountRequest in accountRequests)
                {
                    try
                    {
                        var response = new ApiResponse();

                        var account = await _accountRepository.DetailsAsync(accountRequest.AccountId);

                        var kyc = await _kycRepository.DetailsByUserAsync(account.UserId);
                        if (kyc is null)
                        {
                            _logService.LogInfo(nameof(AccountRequestService), nameof(ProcessAsync), $"Kyc details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        var country = await _countryRepository.GetCountryDetailsAsync(kyc.Country);
                        if (country is null)
                        {
                            _logService.LogInfo(nameof(AccountRequestService), nameof(ProcessAsync), $"country details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        if (string.Equals(accountRequest.Currency, Currencies.NGN)) response = await _globusBankService.GenerateAsync(account);
                        if (!string.Equals(accountRequest.Currency, Currencies.NGN)) response = await _infinitusService.CreateGlobalAccountAsync(account, accountRequest.Currency, kyc.Country);

                        if (!response.Success)
                        {
                            accountRequest.Status = AccountStatuses.Error;
                            await _accountRequestRepository.UpdateAsync(accountRequest);

                            _logService.LogInfo(nameof(AccountRequestService), nameof(ProcessAsync), $"country details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        var user = await _userRepository.GetDetailsByUserIdAsync(kyc.UserId);
                        if (user is null)
                        {
                            _logService.LogInfo(nameof(AccountRequestService), nameof(ProcessAsync), $"User details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        accountRequest.Status = AccountStatuses.Processed;
                        await _accountRequestRepository.UpdateAsync(accountRequest);

                        await _emailService.AccountOpeningAsync(user.EmailAddress, user.FirstName, accountRequest.Currency);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(AccountRequestService), nameof(ProcessAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRequestService), nameof(ProcessAsync), ex);
            }
        }
    }
}
