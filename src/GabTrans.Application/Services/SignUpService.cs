using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Notification;


namespace GabTrans.Application.Services
{
    public class SignUpService(ILogService logService, ISmsNotificationService smsService, IFileService fileService, IAuditRepository auditRepository, IEmailNotificationService emailService, IWalletService walletService, IRepresentativeService directorService, ISequenceService sequenceService, IPasswordService securityService, IAccountService accountService, IValidationService validationService, IOneTimePasswordService oneTimeService, IKycRepository onboardingRepository, ISignUpRepository signUpRepository, ICurrencyRepository currencyRepository, IAccountRepository accountRepository, ICountryRepository countryRepository, IBusinessRepository businessRepository) : ISignUpService
    {
        private readonly ILogService _logService = logService;
        private readonly ISmsNotificationService _smsService = smsService;
        private readonly IFileService _fileService = fileService;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IWalletService _walletService = walletService;
        private readonly IRepresentativeService _directorService = directorService;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IPasswordService _securityService = securityService;
        private readonly IAccountService _accountService = accountService;
        private readonly IValidationService _validationService = validationService;
        private readonly IOneTimePasswordService _oneTimeService = oneTimeService;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;
        private readonly IKycRepository _onboardingRepository = onboardingRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;

        //public async Task<ApiResponse> ReinitiateVerificationAsync(CompleteKycRequest request)
        //{
        //    var result = new ApiResponse();
        //    long businessId = 0;
        //    long accountId = 0;
        //    try
        //    {
        //        var kyc = await _signUpRepository.GetKycByUserIdAsync(request.Actor);

        //        var user = await _signUpRepository.GetUserIdAsync(request.Actor);
        //        if (user == null)
        //        {
        //            return new ApiResponse
        //            {

        //                Message = "Invalid user Id"
        //            };
        //        }

        //        string accountName = string.Concat(user.FirstName, " ", user.LastName);
        //        if (request.AccountTypeId == OnboardingAccountTypes.Business) accountName = request.BusinessName;

        //        var doesNameExist = await _accountRepository.DoesNameExistAsync(accountName);
        //        if (!doesNameExist)
        //        {
        //            string destinationOfFunds = string.Empty;
        //            string purpose = string.Empty;

        //            if (request.PurposeOfAccount.Length > 0)
        //            {
        //                foreach (var o in request.PurposeOfAccount)
        //                {
        //                    if (o != request.PurposeOfAccount[0]) purpose = purpose + ", " + o;
        //                    else purpose = o;
        //                }
        //            }

        //            if (request.DestinationOfFunds.Length > 0)
        //            {
        //                foreach (var o in request.DestinationOfFunds)
        //                {
        //                    if (o != request.DestinationOfFunds[0]) destinationOfFunds = destinationOfFunds + ", " + o;
        //                    else destinationOfFunds = o;
        //                }
        //            }

        //            string address = kyc.ResidentialAddress;
        //            if (request.AccountTypeId == OnboardingAccountTypes.Business) address = request.OperatingAddress;

        //            accountId = await _onboardingRepository.CreateAccountAsync(request.AccountName, user.PhoneNumber, user.EmailAddress, request.AccountTypeId, address, destinationOfFunds, purpose, request.TransactionVolume, request.TransactionValue);

        //            await _onboardingRepository.AddUserToAccountAsync(user.Id, accountId);
        //        }

        //        if (request.AccountTypeId == OnboardingAccountTypes.Business)
        //        {
        //            var business = await _businessRepository.BusinessDetailsAsync(request.CompanyNumber, request.BusinessName);
        //            if (business is not null)
        //            {
        //                await _businessRepository.UpdateAsync(request);
        //            }
        //            else
        //            {
        //                businessId = await _businessRepository.SaveAsync(request);

        //                await _onboardingRepository.AddUserToBusinessAsync(user.Id, businessId);
        //            }
        //        }

        //     //   string referenceNumber = kyc.CustomerNumber == null ? _sequenceService.GenerateCustomerNumber(user.Id) : kyc.CustomerNumber;

        //        //Call Jumio service
        //        switch (kyc.CountryCode.ToUpper())
        //        {
        //            case Countries.United_Kingdom:
        //                result = await JumioOnboardingAsync(kyc.UserId, kyc.Uuid, request.IdentificationType, request.Channel, request.IPAddress);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("OnboardingService", "ReinitiateVerification", ex);

        //        result.Message = "Please try again later";
        //    }

        //    return result;
        //}

        //public async Task<ApiResponse> InitiateVerificationAsync(CompleteOnboardingRequest request)
        //{
        //    long accountId = 0;
        //    long businessId = 0;
        //    var result = new ApiResponse();

        //    var kyc = await _signUpRepository.GetKycByUserIdAsync(request.Actor);

        //    var user = await _signUpRepository.GetUserIdAsync(request.Actor);
        //    if (user == null)
        //    {
        //        return new ApiResponse
        //        {

        //            Message = "Invalid user Id"
        //        };
        //    }

        //    string accountName = string.Concat(user.FirstName, " ", user.LastName);
        //    if (request.AccountTypeId == OnboardingAccountTypes.Business) accountName = request.BusinessName;

        //    var accountDetails = await _accountRepository.DetailsAsync(accountName, user.EmailAddress, user.PhoneNumber);
        //    if (accountDetails == null)
        //    {
        //        string purpose = string.Empty;
        //        string destinationOfFunds = string.Empty;
        //        if (request.PurposeOfAccount.Length > 0)
        //        {
        //            foreach (var o in request.PurposeOfAccount)
        //            {
        //                if (o != request.PurposeOfAccount[0]) purpose = purpose + ", " + o;
        //                else purpose = o;
        //            }
        //        }

        //        if (request.DestinationOfFunds.Length > 0)
        //        {
        //            foreach (var o in request.DestinationOfFunds)
        //            {
        //                if (o != request.DestinationOfFunds[0]) destinationOfFunds = destinationOfFunds + ", " + o;
        //                else destinationOfFunds = o;
        //            }
        //        }

        //        if (kyc.CountryCode == Countries.Nigeria) destinationOfFunds = Countries.Nigeria;

        //        string address = kyc.ResidentialAddress;
        //        if (request.AccountTypeId == OnboardingAccountTypes.Business) address = request.OperatingAddress;

        //        accountId = await _onboardingRepository.CreateAccountAsync(accountName, user.PhoneNumber, user.EmailAddress, request.AccountTypeId, address, destinationOfFunds, purpose, request.TransactionVolume, request.TransactionValue, true);


        //        await _onboardingRepository.AddUserToAccountAsync(user.Id, accountId);
        //    }

        //    if (request.AccountTypeId == OnboardingAccountTypes.Business && !kyc.Confirmed)
        //    {

        //        businessId = await _businessRepository.SaveAsync(request);

        //        await _onboardingRepository.AddUserToBusinessAsync(user.Id, businessId);

        //        await _onboardingRepository.AddBusinessToAccountAsync(accountId, businessId);

        //        await _directorService.CreateAsync(user.Id, businessId, request.CompanyNumber);

        //        await _onboardingRepository.NotifyShareholdersAsync(request);
        //    }

        //    string referenceNumber = _sequenceService.GenerateCustomerNumber(user.Id);

        //    //Call Jumio service
        //    switch (kyc.CountryCode.ToUpper())
        //    {
        //        case Countries.United_Kingdom:
        //            result = await JumioOnboardingAsync(kyc.UserId, referenceNumber, request.IdentificationType, request.Channel, request.IPAddress, businessId);
        //            break;
        //        case Countries.Nigeria:
        //            result = await DojahOnboardingAsync(kyc.UserId, request.IdentificationType, businessId);
        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}


        //public async Task<ApiResponse> UpgradeKycAsync(long userId, string countryCode)
        //{
        //    long businessId = 0;
        //    var result = new ApiResponse();

        //    var kyc = await _signUpRepository.GetKycByUserIdAsync(userId);

        //    var user = await _signUpRepository.GetUserIdAsync(userId);
        //    if (user == null)
        //    {
        //        return new ApiResponse
        //        {

        //            Message = "Invalid user Id"
        //        };
        //    }

        //    //string referenceNumber = _sequenceService.GenerateCustomerNumber(user.Id);

        //    //Call Jumio service
        //    switch (countryCode.ToUpper())
        //    {
        //        //case Countries.United_Kingdom:
        //        //    result = await JumioOnboardingAsync(kyc.UserId, referenceNumber, request.IdentificationType, request.Channel, request.IPAddress, businessId);
        //        //    break;
        //        case Countries.Nigeria:
        //            result = await DojahOnboardingAsync(kyc.UserId, "", businessId);
        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}

        //public async Task<ApiResponse> JumioOnboardingAsync(long userId, string referenceNumber, string identificationType, long channelId, string ipAddress, long? businessId = null)
        //{
        //    long? directorId = null;
        //    var result = new ApiResponse();

        //    try
        //    {
        //        var kyc = await _signUpRepository.GetKycByUserIdAsync(userId);

        //        var country = await _countryRepository.DetailsAsync(kyc.CountryCode);

        //        string uuid = Guid.NewGuid().ToString();

        //        string document = JumioIdTypes.IdCard;

        //        switch (identificationType)
        //        {
        //            case Identifications.Passport:
        //                document = JumioIdTypes.Passport;
        //                break;
        //            case Identifications.Driver_License:
        //                document = JumioIdTypes.Driving_License;
        //                break;
        //            case Identifications.National_Id:
        //                document = JumioIdTypes.IdCard;
        //                break;
        //            case Identifications.Resident_Permit:
        //                document = JumioIdTypes.IdCard;
        //                break;
        //            default:
        //                break;
        //        }

        //        var initiateJumio = new JumioInitiateAccountRequest
        //        {
        //            customerInternalReference = uuid,
        //            workflowDefinition = new AccountWorkflowDefinition
        //            {
        //                credentials = new List<AccCredential> { new AccCredential { category=JumioIdCategories.IdCard,
        //                               type= new JumioAccountType { predefinedType = JumioPredefineTypes.Defined, values= new List<string>{ document }   },
        //                     country= new JumioCountry { predefinedType = JumioPredefineTypes.Defined, values=new List<string>{ country.Code2 } }
        //                }
        //                }
        //            },
        //            userReference = referenceNumber,
        //            web = new WebSetting
        //            {
        //                locale = country.Language
        //            },
        //            userConsent = new JumioUserConsent
        //            {
        //                userIp = ipAddress,
        //                userLocation = new JumioUserLocation { country = country.Code2 },
        //            }
        //        };

        //        if (businessId != null && businessId > 0)
        //        {
        //            var directors = await _directorRepository.UseBusinessIdAsync((long)businessId);
        //            if (directors != null) directorId = directors.Id;
        //        }

        //        var kycRequest = new KycRequestResponse
        //        {
        //            CreatedAt = DateTime.Now,
        //            ReferenceNumber = referenceNumber,
        //            Uuid = uuid,
        //            Status = ScreeningStatuses.Pending,
        //            UserId = userId,
        //            Director = directorId > 0,
        //            DirectorId = directorId,
        //            Request = JsonConvert.SerializeObject(initiateJumio)
        //        };
        //        await _kycRequestRepository.SaveAsync(kycRequest);

        //        var initiateKyc = await _jumioClientIntegration.InitiateAccountAsync(initiateJumio);
        //        if (initiateKyc == null || initiateKyc.sdk == null)
        //        {
        //            return new ApiResponse
        //            {

        //                Message = "Unable to set up verification"
        //            };
        //        }
        //        var updateKyc = await _kycRequestRepository.DetailsAsync(kycRequest.Id);
        //        updateKyc.OnboardingId = initiateKyc.account.id;
        //        updateKyc.Response = JsonConvert.SerializeObject(initiateKyc);
        //        await _kycRequestRepository.UpdateAsync(kycRequest.Id, JsonConvert.SerializeObject(initiateKyc));

        //        var onboardingobject = new OnboardingObject { EmailVerified = kyc.EmailVerified, PhoneVerified = kyc.PhoneVerified, UserId = kyc.UserId, Token = channelId == (long)Channels.Mobile ? initiateKyc.sdk.token : initiateKyc.web.href, Verified = kyc.Verified, Completed = kyc.Completed };

        //        return new ApiResponse
        //        {
        //            Success = true,

        //            Message = "Onboarding successful, kindly complete the verification",
        //            Data = onboardingobject
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("OnboardingService", "JumioOnboarding", ex);

        //        result.Message = "Please try again later";
        //    }

        //    return result;
        //}

        //public async Task<ApiResponse> DojahOnboardingAsync(long userId, string identificationType, long? businessId = null)
        //{
        //    long? directorId = null;

        //    //var kyc = await _signUpRepository.GetKycByUserIdAsync(userId);

        //    string uuid = Guid.NewGuid().ToString();

        //    //string document = DojahIdTypes.National_Id_Card;

        //    //switch (identificationType)
        //    //{
        //    //    case Identifications.Passport:
        //    //        document = DojahIdTypes.Passport;
        //    //        break;
        //    //    case Identifications.Driver_License:
        //    //        document = DojahIdTypes.Driver_License;
        //    //        break;
        //    //    case Identifications.Voter_Id_No:
        //    //        document = DojahIdTypes.Voter_Id_No;
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    if (businessId != null && businessId > 0)
        //    {
        //        var directors = await _directorRepository.UseBusinessIdAsync((long)businessId);
        //        if (directors != null) directorId = directors.Id;
        //    }

        //    var kycRequest = new KycRequestResponse
        //    {
        //        CreatedAt = DateTime.Now,
        //        ReferenceNumber = uuid,
        //        Uuid = uuid,
        //        Status = ScreeningStatuses.Pending,
        //        UserId = userId,
        //        Director = directorId > 0,
        //        DirectorId = directorId
        //    };
        //    await _kycRequestRepository.SaveAsync(kycRequest);

        //    return new ApiResponse
        //    {
        //        Success = true,

        //        Message = "Please proceed to take your sefie",
        //        Data = uuid
        //    };
        //}

        //public async Task CreateAccountAsync()
        //{
        //    try
        //    {
        //        var accounts = await _accountRepository.WithoutAccountNumberAsync();

        //        foreach (var a in accounts)
        //        {
        //            var userAccount = await _accountRepository.GetAccountsAsync(a.Id);
        //            if (userAccount == null) continue;

        //            var kyc = await _signUpRepository.GetKycByUserIdAsync(userAccount.UserId);

        //            var country = await _countryRepository.DetailsAsync(kyc.CountryCode);

        //            var currency = await _currencyRepository.DetailsByCountryAsync(kyc.CountryCode.ToUpper());

        //            await _walletService.CreateAsync(a.Id, currency.Code);

        //            switch (kyc.CountryCode.ToUpper())
        //            {
        //                case Countries.United_Kingdom:
        //                    await _currencyCloudService.RegisterAsync(kyc.UserId, a.Id, kyc.Uuid, country.Id);
        //                    break;
        //                case Countries.Nigeria:
        //                    await _budPayService.CreateAccountAsync(kyc.UserId, a.Id);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("OnboardingService", "CreateAccount", ex);
        //    }
        //}

        //public async Task UpdateAccount()
        //{
        //    try
        //    {
        //        var accounts = await _accountRepository.WithAccountNumberAsync();

        //        foreach (var a in accounts)
        //        {
        //            var userAccount = await _accountRepository.GetUserAccountAsync(a.Id);
        //            if (userAccount == null) continue;

        //            var kyc = await _signUpRepository.GetKycByUserIdAsync(userAccount.UserId);

        //            var country = await _countryRepository.DetailsAsync(kyc.CountryCode);

        //            switch (kyc.CountryCode.ToUpper())
        //            {
        //                case Countries.United_Kingdom:
        //                    await _currencyCloudService.UpdateAccountAsync(kyc.UserId, a.Id, kyc.CustomerNumber, country.Id);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("OnboardingService", "UpdateAccount", ex);
        //    }
        //}


        public async Task TestMethod()
        {
            try
            {
                var kycs = await _onboardingRepository.GetKycsAsync();
                foreach (var k in kycs)
                {
                    var user = await _signUpRepository.GetUserIdAsync(k.UserId);
                    if (user is null) continue;
                    // await _accountService.OpenAccountAsync(user);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("OnboardingService", "TestMethod", ex);
            }
        }

        //public async Task<string> JumioDirectorOnboardingAsync(long directorId, string referenceNumber, Channels channel, string ipAddress)
        //{
        //    var details = await _directorRepository.DetailsByIdAsync(directorId);

        //    var country = await _countryRepository.DetailsAsync(details.CountryCode);

        //    string uuid = Guid.NewGuid().ToString();

        //    string document = JumioIdTypes.IdCard;

        //    var initiateJumio = new JumioInitiateAccountRequest
        //    {
        //        customerInternalReference = uuid,
        //        workflowDefinition = new AccountWorkflowDefinition
        //        {
        //            credentials = new List<AccCredential> { new AccCredential { category=JumioIdCategories.IdCard,
        //                               type= new JumioAccountType { predefinedType = JumioPredefineTypes.Defined, values= new List<string>{ document }   },
        //                     country= new JumioCountry { predefinedType = JumioPredefineTypes.Defined, values=new List<string>{ country.Code2 } }
        //                }
        //                }
        //        },
        //        userReference = referenceNumber,
        //        web = new WebSetting
        //        {
        //            locale = country.Language
        //        },
        //        userConsent = new JumioUserConsent
        //        {
        //            userIp = ipAddress,
        //            userLocation = new JumioUserLocation { country = country.Code2 },
        //        }
        //    };

        //    var kycRequest = new KycRequestResponse
        //    {
        //        CreatedAt = DateTime.Now,
        //        ReferenceNumber = referenceNumber,
        //        Uuid = uuid,
        //        Status = ScreeningStatuses.Pending,
        //        DirectorId = directorId,
        //        Director = true,
        //        Request = JsonConvert.SerializeObject(initiateJumio)
        //    };
        //    await _kycRequestRepository.SaveAsync(kycRequest);

        //    var initiateKyc = await _jumioClientIntegration.InitiateAccountAsync(initiateJumio);
        //    if (initiateKyc == null || initiateKyc.sdk == null) return string.Empty;

        //    await _kycRequestRepository.UpdateAsync(kycRequest.Id, initiateKyc.account.id, JsonConvert.SerializeObject(initiateKyc));

        //    return channel == Channels.Mobile ? initiateKyc.sdk.token : initiateKyc.web.href;
        //}

        //public async Task<ApiResponse> ValidateBvnAsync(string firstName, string lastName, DateTime dateOfBirth, string bvn)
        //{
        //    var result = new ApiResponse();
        //    bool isBVNValid = false;
        //    string firstDob = dateOfBirth.ToString("dd-MMM-yyyy");
        //    string secondDob = dateOfBirth.ToString("yyyy-MM-dd");
        //    string thirdDob = dateOfBirth.ToString("yyyy-MMM-dd");

        //    var bvnLookUp = await _dojahService.BVNLookUpAsync(bvn);
        //    if ((bvnLookUp is null || bvnLookUp.entity is null) && !Convert.ToBoolean(StaticData.Domain.Equals("")))
        //    {
        //        return new ApiResponse
        //        {

        //            Message = "Validation failed"
        //        };
        //    }

        //    //if (!Convert.ToBoolean(_appSettings.Value) && (firstName.Trim().ToLower().Equals(bvnLookUp.entity.first_name.ToLower()) || firstName.Trim().ToLower().Equals(bvnLookUp.entity.last_name.ToLower()) || firstName.Trim().ToLower().Equals(bvnLookUp.entity.middle_name.ToLower())) && (lastName.Trim().ToLower().Equals(bvnLookUp.entity.last_name.ToLower()) || lastName.Trim().ToLower().Equals(bvnLookUp.entity.first_name.ToLower()) || lastName.Trim().ToLower().Equals(bvnLookUp.entity.middle_name.ToLower())) && (bvnLookUp.entity.date_of_birth.Equals(firstDob) || bvnLookUp.entity.date_of_birth.Equals(secondDob) || bvnLookUp.entity.date_of_birth.Equals(thirdDob))) isBVNValid = true;
        //    //if (!isBVNValid && !Convert.ToBoolean(_appSettings.Value))
        //    //{
        //    //    return new AppResult
        //    //    {
        //    //        Returned
        //    //        ResponseMessage = "Validation failed"
        //    //    };
        //    //}

        //    result.Success = true;

        //    result.Message = "Success";
        //    return result;
        //}

        //public async Task<ApiResponse> ValidateBvnAsync(long userId, string firstName, string lastName, DateTime dateOfBirth, string bvn)
        //{
        //    var result = new ApiResponse();
        //    bool isBVNValid = false;
        //    string stateOfOrigin = string.Empty;
        //    string firstDob = dateOfBirth.ToString("dd-MMM-yyyy");
        //    string secondDob = dateOfBirth.ToString("yyyy-MM-dd");
        //    string thirdDob = dateOfBirth.ToString("yyyy-MMM-dd");

        //    //var bvnLookUp = await _dojahService.BVNLookUpAsync(bvn);
        //    //if ((bvnLookUp is null || bvnLookUp.entity is null) && !Convert.ToBoolean(_appSettings.Value))
        //    //{
        //    //    return new AppResult
        //    //    {
        //    //        Returned
        //    //        ResponseMessage = "Validation failed"
        //    //    };
        //    //}

        //    //if (!Convert.ToBoolean(_appSettings.Value) && (firstName.Trim().ToLower().Equals(bvnLookUp.entity.first_name.ToLower()) || firstName.Trim().ToLower().Equals(bvnLookUp.entity.last_name.ToLower()) || firstName.Trim().ToLower().Equals(bvnLookUp.entity.middle_name.ToLower())) && (lastName.Trim().ToLower().Equals(bvnLookUp.entity.last_name.ToLower()) || lastName.Trim().ToLower().Equals(bvnLookUp.entity.first_name.ToLower()) || lastName.Trim().ToLower().Equals(bvnLookUp.entity.middle_name.ToLower())) && (bvnLookUp.entity.date_of_birth.Equals(firstDob) || bvnLookUp.entity.date_of_birth.Equals(secondDob) || bvnLookUp.entity.date_of_birth.Equals(thirdDob))) isBVNValid = true;

        //    //if (bvnLookUp.entity.state_of_origin.Contains("State")) stateOfOrigin = bvnLookUp.entity.state_of_origin.Replace(" State", "");

        //    //await _onboardingRepository.SaveBvnDetailsAsync(userId, isBVNValid, stateOfOrigin);

        //    //if (!isBVNValid && !Convert.ToBoolean(_appSettings.Value))
        //    //{

        //    //    return new AppResult
        //    //    {
        //    //        Returned
        //    //        ResponseMessage = "BVN does not match your details"
        //    //    };
        //    //}

        //    result.Success = true;

        //    result.Message = "Success";
        //    return result;
        //}

        public async Task<ApiResponse> EmailVerificationAsync(User user)
        {
            string onetimepassword = string.Empty;

            var kyc = await _signUpRepository.GetKycByUserIdAsync(user.Id);
            if (kyc != null && !kyc.VerifyEmail)
            {
                var oneTimePassword = await _oneTimeService.GenerateAsync(user.Id, (long)OTPCategories.EmailVerification);
                if (!oneTimePassword.Success)
                {
                    return new ApiResponse
                    {
                        Message = "Kindly try again later"
                    };
                }
                onetimepassword = oneTimePassword.Data.ToString();
                //Push to Mail service
                _ = await _emailService.EmailVerificationAsync(user.EmailAddress, oneTimePassword.Data.ToString());
            }

            var onboardingObj = new OnboardingObject { UserId = user.Id, EmailVerified = kyc != null && kyc.VerifyEmail, OTP = onetimepassword };

            return new ApiResponse { Success = true, Message = "Successful, kindly confirm your registration using the OTP", Data = onboardingObj };
        }
    }
}
