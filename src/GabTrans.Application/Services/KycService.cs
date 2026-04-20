using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using Newtonsoft.Json;


namespace GabTrans.Application.Services
{
    public class KycService(ILogService logService, IEmailNotificationService emailService, IKycRepository kycRepository, IInfinitusService infinitusService, ICurrencyRepository currencyRepository, ICountryRepository countryRepository, IAccountRepository accountRepository, IBusinessRepository businessRepository, IUserRepository userRepository, IKycRequestRepository kycRequestRepository) : IKycService
    {
        private readonly ILogService _logService = logService;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IKycRequestRepository _kycRequestRepository = kycRequestRepository;

        public async Task SubmitAsync()
        {
            try
            {
                var kycs = await _kycRepository.GetAsync(KycStatuses.Approved, Countries.Nigeria);
                foreach (var kyc in kycs)
                {
                    try
                    {
                        ApiResponse? response;

                        var kycRequest = await _kycRequestRepository.DetailsByUserAsync(kyc.UserId);

                        var user = await _userRepository.GetDetailsByUserIdAsync(kyc.UserId);
                        if (user is null)
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);
                            _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"User details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        var business = await _businessRepository.GetByUserAsync(kyc.UserId);
                        if (business is null && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);

                            _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"business details not found for user ID {kyc.UserId}");
                            continue;
                        }

                        if (!string.IsNullOrEmpty(kyc.Uuid) && !string.Equals(kyc.Status, KycStatuses.Approved, StringComparison.OrdinalIgnoreCase))
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);

                            _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to create client for user ID {kyc.UserId}");
                            continue;
                        }

                        if (string.IsNullOrEmpty(kyc.Uuid))
                        {
                            response = await _infinitusService.CreateClientAsync(user, kyc.Type);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to create client for user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (string.Equals(kyc.Type, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase) && !kyc.DataUploaded)
                        {
                            response = await _infinitusService.UpdatePersonalClientAsync(user);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to update client for user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (string.Equals(kyc.Type, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase) && !kyc.DocumentUploaded)
                        {
                            //Upload documents
                            response = await _infinitusService.DocumentClientAsync(kyc);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to upload documents for client with user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (!string.Equals(kyc.Status, KycStatuses.Completed, StringComparison.OrdinalIgnoreCase) && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && business is not null && !business.DataUploaded)
                        {
                            response = await _infinitusService.UpdateBusinessClientAsync(business, kyc.UserId);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to update client with user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (!string.Equals(kyc.Status, KycStatuses.Completed, StringComparison.OrdinalIgnoreCase) && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && business is not null && !business.DocumentUploaded)
                        {
                            response = await _infinitusService.DocumentBusinessClientAsync(kyc);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to upload documents for business client with user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (!string.Equals(kyc.Status, KycStatuses.Completed, StringComparison.OrdinalIgnoreCase) && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && !kyc.DataUploaded)
                        {
                            response = await _infinitusService.CreateRepresentativeAsync(user);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to create representatives for client with user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        if (!string.Equals(kyc.Status, KycStatuses.Completed, StringComparison.OrdinalIgnoreCase) && string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && !kyc.DocumentUploaded)
                        {
                            //Upload documents
                            response = await _infinitusService.DocumentRepresentativeAsync(kyc);
                            if (!response.Success)
                            {
                                kycRequest.Status = KycStatuses.Error;
                                await _kycRequestRepository.UpdateAsync(kycRequest);

                                _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to upload documents for client representative with user ID {kyc.UserId}");
                                continue;
                            }
                        }

                        List<string> providers = [.. InfinitusPayProviders.Ssb.Split(',')];

                        response = await _infinitusService.SubmitClientAsync(kyc, providers);
                        if (!response.Success)
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);

                            _logService.LogInfo(nameof(KycService), nameof(SubmitAsync), $"Unable to submit application for client with user ID {kyc.UserId}");
                            continue;
                        }

                        kyc.Status = KycStatuses.Completed;
                        await _kycRepository.UpdateAsync(kyc);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(KycService), nameof(SubmitAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycService), nameof(SubmitAsync), ex);
            }
        }

        public async Task UpdateAsync()
        {
            try
            {
                var kycs = await _kycRepository.GetCompletedAsync(KycStatuses.Completed, Countries.Nigeria);
                foreach (var kyc in kycs)
                {
                    try
                    {
                        var kycRequest = await _kycRequestRepository.DetailsByUserAsync(kyc.UserId);

                        var response = await _infinitusService.GetAccountRequestAsync(kyc);
                        if (!response.Success)
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);

                            _logService.LogInfo(nameof(KycService), nameof(UpdateAsync), $"User details not found for user ID {kyc.UserId}::" + JsonConvert.SerializeObject(response));
                            continue;
                        }

                        var accountResponse = (InfinitusAccountRequestResponse)response.Data;
                        if(accountResponse is null)
                        {
                            kycRequest.Status = KycStatuses.Error;
                            await _kycRequestRepository.UpdateAsync(kycRequest);

                            _logService.LogInfo(nameof(KycService), nameof(UpdateAsync), $"Unable to fetch account for user ID {kyc.UserId}::" + JsonConvert.SerializeObject(response));
                            continue;
                        }

                        var account = await _accountRepository.GetAccountAsync(kyc.UserId);
                        if (account is not null)
                        {
                            account.Uuid = accountResponse.accountId;
                            account.Provider = accountResponse.provider;
                            bool update = await _accountRepository.UpdateAsync(account);
                            if (!update)
                            {
                                _logService.LogInfo(nameof(KycService), nameof(UpdateAsync), $"Unable to create account for user ID {kyc.UserId}::" + JsonConvert.SerializeObject(response));
                                continue;
                            }
                        }

                        kyc.Status = KycStatuses.Passed;
                        await _kycRepository.UpdateAsync(kyc);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(KycService), nameof(UpdateAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycService), nameof(UpdateAsync), ex);
            }
        }

        public async Task<ApiResponse> UpdateAddressAsync(UpdateAddressRequest request, long userId)
        {
            string accountType = AccountTypes.Personal;

            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "User details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Kyc detail not found"
                };
            }

            var business = await _businessRepository.GetByUserAsync(userId);
            if (business is not null) accountType = AccountTypes.Business;

            kyc.Address1 = request.Address1;
            kyc.Address2 = request.Address2;
            kyc.City = request.City;
            kyc.Country = request.Country;
            kyc.PostalCode = request.PostalCode;
            kyc.ResidentialState = request.State;
            ////kyc.UpdateAddress = true;
            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase)) kyc.Status = KycStatuses.Submitted;

            //bool update = await _kycRepository.UpdateAsync(kyc);
            //if (!update)
            //{
            //    return new ApiResponse
            //    {
            //        Message = " Unable to update address details"
            //    };
            //}

            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase))
            //{
            //    await _kycRequestRepository.UpdateAsync(userId, KycStatuses.Submitted);

            //    string fullName = $"{user.FirstName} {user.LastName}";

            //    await _emailService.AccountRequestAsync(fullName, accountType, user.CreatedAt);
            //}

            return new ApiResponse { Message = "Updated the address details successfully", Success = true };
        }

        public async Task<ApiResponse> UpdateEmploymentAsync(UpdateEmploymentRequest request, long userId)
        {
            string accountType = AccountTypes.Personal;

            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "User details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Kyc detail not found"
                };
            }

            var business = await _businessRepository.GetByUserAsync(userId);
            if (business is not null) accountType = AccountTypes.Business;

            kyc.EmploymentStatus = request.EmploymentStatus;
            kyc.Occupation = request.Occupation;
            kyc.OccupationDescription = request.OccupationDescription;
            kyc.Employer = request.Employer;
            kyc.EmployerState = request.EmployerState;
            kyc.EmployerCountry = request.EmployerCountry;
            kyc.Industry = request.Industry;
            kyc.UpdateEmployment = true;
            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase)) kyc.Status = KycStatuses.Submitted;

            //bool update = await _kycRepository.UpdateAsync(kyc);
            //if (!update)
            //{
            //    return new ApiResponse
            //    {
            //        Message = " Unable to update employment details"
            //    };
            //}

            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase))
            //{
            //    await _kycRequestRepository.UpdateAsync(userId, KycStatuses.Submitted);

            //    string fullName = $"{user.FirstName} {user.LastName}";

            //    await _emailService.AccountRequestAsync(fullName, accountType, user.CreatedAt);
            //}

            return new ApiResponse { Message = "Updated the employment details successfully", Success = true };
        }

        public async Task<ApiResponse> UpdateIdentityAsync(UpdateIdentityRequest request, long userId)
        {
            string accountType = AccountTypes.Personal;

            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "User details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Kyc detail not found"
                };
            }

            var business = await _businessRepository.GetByUserAsync(userId);
            if (business is not null) accountType = AccountTypes.Business;

            kyc.IdentityDocumentFront = request.IdentityDocumentFront;
            if (!string.IsNullOrEmpty(request.IdentityDocumentBack)) kyc.IdentityDocumentBack = request.IdentityDocumentBack;
            kyc.IdentityExpiryDate = request.IdentityExpiryDate;
            kyc.IdentityIssueDate = request.IdentityIssueDate;
            kyc.IdentityNumber = request.IdentityNumber;
            kyc.IdentityType = request.IdentityType;
            kyc.UpdateIdentity = true;
            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase)) kyc.Status = KycStatuses.Submitted;

            //bool update = await _kycRepository.UpdateAsync(kyc);
            //if (!update)
            //{
            //    return new ApiResponse
            //    {
            //        Message = " Unable to update user identity"
            //    };
            //}

            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase))
            //{
            //    await _kycRequestRepository.UpdateAsync(userId, KycStatuses.Submitted);

            //    string fullName = $"{user.FirstName} {user.LastName}";

            //    await _emailService.AccountRequestAsync(fullName, accountType, user.CreatedAt);
            //}

            return new ApiResponse { Message = "Updated the identification details successfully", Success = true };
        }

        public async Task<ApiResponse> UpdateIncomeAsync(UpdateIncomeRequest request, long userId)
        {
            string accountType = AccountTypes.Personal;

            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Message = "User details not found"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc == null)
            {
                return new ApiResponse
                {
                    Message = "Kyc detail not found"
                };
            }

            if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(request.Role))
            {
                return new ApiResponse
                {
                    Message = "Please supply the role"
                };
            }

            if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(request.Title))
            {
                return new ApiResponse
                {
                    Message = "Please supply the title"
                };
            }

            var business = await _businessRepository.GetByUserAsync(userId);
            if (business is not null) accountType = AccountTypes.Business;

            kyc.AnnualIncome = request.AnnualIncome;
            kyc.IncomeCountry = request.IncomeCountry;
            kyc.IncomeSource = request.IncomeSource;
            kyc.IncomeState = request.IncomeState;
            kyc.WealthSource = request.WealthSource;
            kyc.SourceOfFund = request.SourceOfFunds;
            kyc.WealthSourceDescription = request.WealthSourceDescription;
            kyc.Role = request.Role;
            kyc.Title = request.Title;
            kyc.IsSigner = request.IsSigner;
            kyc.OwnershipPercentage = request.OwnershipPercentage;
            //kyc.UpdateIncome = true;
            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase)) kyc.Status = KycStatuses.Submitted;

            //bool update = await _kycRepository.UpdateAsync(kyc);
            //if (!update)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to update user income"
            //    };
            //}

            //if (kyc.UpdatePersonal && kyc.UpdateIncome && kyc.UpdateIdentity && kyc.UpdateEmployment && string.Equals(accountType, AccountTypes.Personal, StringComparison.OrdinalIgnoreCase))
            //{
            //    await _kycRequestRepository.UpdateAsync(userId, KycStatuses.Submitted);

            //    string fullName = $"{user.FirstName} {user.LastName}";

            //    await _emailService.AccountRequestAsync(fullName, accountType, user.CreatedAt);
            //}

            return new ApiResponse { Message = "Updated income details successfully", Success = true };
        }
    }
}
