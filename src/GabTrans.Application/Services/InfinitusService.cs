using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Newtonsoft.Json;


namespace GabTrans.Application.Services
{
    public class InfinitusService(ILogService logService, IFileService fileService, IKycRepository kycRepository, IUserRepository userRepository, ITransferRepository payoutRepository, IWalletRepository walletRepository, IAccountRepository accountRepository, ICountryRepository countryRepository, IBusinessRepository businessRepository, IRecipientRepository recipientRepository, ICryptoTradeRepository cryptoTradeRepository, IBusinessTeamRepository businessTeamRepository, IVirtualAccountRepository virtualAccountRepository, IInfinitusClientIntegration infinitusClientIntegration) : IInfinitusService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITransferRepository _payoutRepository = payoutRepository;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IRecipientRepository _recipientRepository = recipientRepository;
        private readonly ICryptoTradeRepository _cryptoTradeRepository = cryptoTradeRepository;
        private readonly IBusinessTeamRepository _businessTeamRepository = businessTeamRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository = virtualAccountRepository;
        private readonly IInfinitusClientIntegration _infinitusClientIntegration = infinitusClientIntegration;

        public async Task<ApiResponse> CreateClientAsync(User user, string type)
        {
            string businessName = string.Empty;

            var kyc = await _kycRepository.DetailsByUserAsync(user.Id);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve KYC"
                };
            }
            if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase))
            {
                var business = await _businessRepository.GetByUserAsync(user.Id);
                if (business is null)
                {
                    return new ApiResponse
                    {
                        Message = "No business details found"
                    };
                }

                businessName = business.Name;
            }

            var clientRequest = new CreateInfinitusClientRequest
            {
                contactEmail = user.EmailAddress,
                contactFirstName = user.FirstName,
                contactLastName = user.LastName,
                name = businessName,
                type = type.ToUpper(),
                useHostedFlow = false
            };
            var response = await _infinitusClientIntegration.CreateClientAsync(clientRequest);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create profile"
                };
            }

            //Update the KYC column
            kyc.Uuid = response.id;
            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update KYC"
                };
            }

            return new ApiResponse { Success = true, Message = "Profile created successfully" };
        }

        public async Task<ApiResponse> CreateRecipientAsync(TransferRecipient recipient, long accountId)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            if (string.IsNullOrEmpty(account.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            string firstName = string.Empty;
            string lastName = string.Empty;
            string[] names = recipient.Name.Split(" ");
            if (names.Length > 0) firstName = names[0];
            if (names.Length > 1) lastName = names[1];

            var clientRequest = new CreateInfinitusRecipientRequest
            {
                address = new InfinitusRecipientAddress
                {
                    city = recipient.City,
                    countryCode = recipient.Country,
                    streetAddress = recipient.StreetAddress,
                    postCode = recipient.PostCode,
                    state = recipient.State
                },
                intermediaryBankAddress = new InfinitusRecipientIntermediaryBankAddress
                {
                    city = recipient.IntermediaryCity,
                    country = recipient.IntermediaryBankCountry,
                    postalCode = recipient.IntermediaryPostalCode,
                    state = recipient.IntermediaryState,
                    street1 = recipient.IntermediaryStreet1,
                    //street2 = recipient.IntermediaryStreet2
                },
                recipientAccountType = recipient.Type.ToUpper(),
                paymentMethod = recipient.PaymentMethod.ToUpper(),
                lastName = lastName,
                firstName = firstName,
                companyName = recipient.AccountName,
                intermediaryRoutingCode = recipient.IntermediaryRoutingCode,
                intermediaryBankName = recipient.IntermediaryBankName,
                internationalBankName = recipient.InternationalBankName,
                bankDetails = new InfinitusRecipientBankDetails
                {
                    accountCurrency = recipient.Currency,
                    accountName = recipient.AccountName,
                    accountNumber = recipient.AccountNumber,
                    accountRoutingType1 = recipient.AccountRoutingType,
                    bankAccountType = recipient.BankAccountType.ToUpper(),
                    // bankBranch = recipient.BankBranch,
                    bankCity = recipient.BankCity,
                    bankCountryCode = recipient.BankCountry,
                    bankName = recipient.BankName,
                    bankPostalCode = recipient.BankPostalCode,
                    bankState = recipient.BankState,
                    bankStreetAddress = recipient.BankStreetAddress,
                    iban = recipient.AccountNumber,
                    routingNumber = recipient.RoutingNumber,
                    swiftCode = recipient.SwiftCode
                },
                additionalInfo = new InfinitusRecipientAdditionalInfo
                {
                    businessPhoneNumber = recipient.PhoneNumber,
                    dateOfBirth = recipient.DateOfBirth == null ? null : recipient.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    email = recipient.Email,
                    personalMobileNumber = recipient.PhoneNumber
                }
            };
            var response = await _infinitusClientIntegration.CreateRecipientAsync(clientRequest, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create profile"
                };
            }

            //Update the Uuid column
            recipient.Uuid = response.id;
            bool update = await _recipientRepository.UpdateAsync(recipient);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update payout Recipient"
                };
            }

            return new ApiResponse { Success = true, Message = response.id };
        }

        public async Task<ApiResponse> CreateRepresentativeAsync(User user)
        {
            var kyc = await _kycRepository.DetailsByUserAsync(user.Id);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve KYC"
                };
            }

            //if (!string.IsNullOrEmpty(kyc.Uuid))
            //{
            //    return new ApiResponse
            //    {
            //         Success=true,
            //        Message = "Profile created successfully"
            //    };
            //}

            var businessTeam = await _businessTeamRepository.GetByUserIdAsync(user.Id);
            if (businessTeam is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch details of teams"
                };
            }

            if (!string.Equals(kyc.Country, Countries.United_Kingdom, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(kyc.TaxNumber) && !kyc.TaxNumber.Contains('-'))
            {
                kyc.TaxNumber = $"{kyc.TaxNumber[..2]}-{kyc.TaxNumber[2..]}";
                // kyc.TaxNumber = $"{kyc.TaxNumber[..3]}-{kyc.TaxNumber[3..5]}-{kyc.TaxNumber[5..]}";
            }

            var clientRequest = new RepresentativeInfinitusRequest
            {
                address = new InfinitusAddress
                {
                    city = kyc.City,
                    country = kyc.Country,
                    line1 = kyc.Address1,
                    line2 = kyc.Address2,
                    postalCode = kyc.PostalCode,
                    state = kyc.ResidentialState.Substring(2)
                },
                wealthSourceDescription = kyc.WealthSourceDescription,
                annualIncome = kyc.AnnualIncome,
                citizenship = kyc.Citizenship,
                dateOfBirth = kyc.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                email = user.EmailAddress,
                employer = kyc.Employer,
                employerCountry = kyc.EmployerCountry,
                employerState = kyc.EmployerState.Substring(2),
                employmentStatus = kyc.EmploymentStatus,
                firstName = user.FirstName,
                idDocument = new InfinitusIdDocument
                {
                    number = kyc.IdentityNumber,
                    type = kyc.IdentityType,
                    expirationDate = kyc.IdentityExpiryDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    issueDate = kyc.IdentityIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd")
                },
                incomeCountry = kyc.IncomeCountry,
                incomeSource = kyc.IncomeSource,
                incomeState = kyc.IncomeState.Substring(2),
                isSigner = kyc.IsSigner,
                industry = kyc.Industry,
                lastName = user.LastName,
                occupation = kyc.Occupation,
                occupationDescription = kyc.OccupationDescription,
                ownershipPercentage = kyc.OwnershipPercentage,
                phone = user.PhoneNumber,
                role = kyc.Role,
                sourceOfFunds = kyc.SourceOfFund,
                taxNumber = kyc.TaxNumber,
                title = kyc.Title,
                wealthSource = kyc.WealthSource
            };
            var response = await _infinitusClientIntegration.CreateRepresentativeAsync(clientRequest, kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create profile"
                };
            }

            //Update the Uuid column
            businessTeam.Uuid = response.id;
            bool update = await _businessTeamRepository.UpdateAsync(businessTeam);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update representative"
                };
            }

            //Update the Uuid column
            kyc.DataUploaded = true;
            update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            return new ApiResponse { Success = true, Message = "Profile created successfully" };
        }

        public async Task<ApiResponse> DocumentClientAsync(Kyc kyc)
        {
            var response = new List<DocumentInfinitusResponse>();

            if (string.Equals(kyc.Type, AccountTypes.Business, StringComparison.OrdinalIgnoreCase)) return await DocumentRepresentativeAsync(kyc);

            string fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

            var fileObject = await _fileService.DownloadFileAsync(fileName);

            if (string.Equals(kyc.IdentityType, Identifications.Passport, StringComparison.OrdinalIgnoreCase))
            {
                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.Passport);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            if (string.Equals(kyc.IdentityType, Identifications.DriversLicense, StringComparison.OrdinalIgnoreCase))
            {
                fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.DriversLicenseFront);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }

                fileName = _fileService.GetFileName(kyc.IdentityDocumentBack);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.DriversLicenseBack);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            if (string.Equals(kyc.IdentityType, Identifications.IdCard, StringComparison.OrdinalIgnoreCase))
            {
                fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.IdCard);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }

                fileName = _fileService.GetFileName(kyc.IdentityDocumentBack);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.IdCardBack);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            if (!string.IsNullOrEmpty(kyc.ProofOfAddress))
            {
                fileName = _fileService.GetFileName(kyc.ProofOfAddress);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.ProofOfAddress);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            // kyc.Status = KycStatuses.Submitted;
            kyc.DocumentUploaded = true;
            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update KYC status"
                };
            }

            return new ApiResponse { Success = true, Message = "File uploaded retrieved successfully" };
        }


        public async Task<ApiResponse> DocumentBusinessClientAsync(Kyc kyc)
        {
            var business = await _businessRepository.GetByUserAsync(kyc.UserId);
            if (business is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for business"
                };
            }

            //Tax document
            string fileName = _fileService.GetFileName(business.TaxDocument);

            var fileObject = await _fileService.DownloadFileAsync(fileName);

            var response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.TaxId);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Bank statement
            fileName = _fileService.GetFileName(kyc.BankStatement);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.BankStatement);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Proof of registration
            fileName = _fileService.GetFileName(business.ProofOfRegistration);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.ProofOfRegistration);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Proof of ownership
            fileName = _fileService.GetFileName(business.ProofOfOwnership);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.ProofOfOwnership);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Formation document
            fileName = _fileService.GetFileName(business.FormationDocument);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.FormationDocument);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Agreement document
            fileName = _fileService.GetFileName(business.Agreement);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentBusinessClientAsync(kyc.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.Agreement);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            business.DocumentUploaded = true;
            bool update = await _businessRepository.UpdateAsync(business);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update Business status"
                };
            }

            return new ApiResponse { Success = true, Message = "File uploaded retrieved successfully" };
        }

        public async Task<ApiResponse> DocumentRepresentativeAsync(Kyc kyc)
        {
            var response = new List<DocumentInfinitusResponse>();

            var representative = await _businessTeamRepository.GetByUserIdAsync(kyc.UserId);
            if (representative is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve details"
                };
            }

            string fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

            var fileObject = await _fileService.DownloadFileAsync(fileName);

            if (string.Equals(kyc.IdentityType, Identifications.Passport, StringComparison.OrdinalIgnoreCase))
            {
                response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.Passport);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            if (string.Equals(kyc.IdentityType, Identifications.DriversLicense, StringComparison.OrdinalIgnoreCase))
            {
                fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.DriversLicenseFront);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }

                fileName = _fileService.GetFileName(kyc.IdentityDocumentBack);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.DriversLicenseBack);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            if (string.Equals(kyc.IdentityType, Identifications.IdCard, StringComparison.OrdinalIgnoreCase))
            {
                fileName = _fileService.GetFileName(kyc.IdentityDocumentFront);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.IdCard);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }

                fileName = _fileService.GetFileName(kyc.IdentityDocumentBack);

                fileObject = await _fileService.DownloadFileAsync(fileName);

                response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.IdCardBack);
                if (response is null || response.Count == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Service is not reachable"
                    };
                }
            }

            fileName = _fileService.GetFileName(kyc.ProofOfAddress);

            fileObject = await _fileService.DownloadFileAsync(fileName);

            response = await _infinitusClientIntegration.DocumentRepresentativeAsync(kyc.Uuid, representative.Uuid, fileObject.Stream, $"{fileName}.{fileObject.Extension}", InfinitusPayIdentityTypes.ProofOfAddress);
            if (response is null || response.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            //Update the Uuid column
            kyc.DocumentUploaded = true;
            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            return new ApiResponse { Success = true, Message = "File uploaded retrieved successfully" };
        }

        public async Task<ApiResponse> GetClientAsync(long userId)
        {
            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve KYC"
                };
            }

            var response = await _infinitusClientIntegration.GetClientAsync(kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch profile"
                };
            }

            return new ApiResponse { Success = true, Message = "Profile retrieved successfully" };
        }

        public async Task<ApiResponse> GetRecipientAsync(long recipientId, long accountId)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            if (string.IsNullOrEmpty(account.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            var recipient = await _recipientRepository.DetailsAsync(recipientId);
            if (recipient is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve recipient"
                };
            }

            if (string.IsNullOrEmpty(recipient.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            var response = await _infinitusClientIntegration.GetRecipientAsync(recipient.Uuid, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to details for recipient"
                };
            }

            return new ApiResponse { Success = true, Message = "Fetched details for Recipient successfully", Data = response };
        }

        public async Task<ApiResponse> GetRecipientSchemaAsync(GetTransferSchemaRequest request, long userId)
        {
            var account = await _accountRepository.GetAccountAsync(userId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            if (string.IsNullOrEmpty(account.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            var schemaRequest = new GetInfinitusRecipientSchemaRequest
            {
                accountCurrency = request.Currency.ToUpper(),
                bankCountryCode = request.BankCountryCode.ToUpper(),
                countryCode = request.CountryCode.ToUpper(),
                recipientAccountType = request.AccountType.ToUpper(),
                transferMethod = request.TransferMethod.ToUpper()
            };
            var response = await _infinitusClientIntegration.GetRecipientSchemaAsync(schemaRequest, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (response.fields.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to process request"
                };
            }

            var schema = GetPaymentSchema(response);

            return new ApiResponse { Success = true, Data = schema, Message = "Requirement retrieved successfully" };
        }

        public static List<string> GetPaymentSchema(GetInfinitusRecipientSchemaResponse response)
        {
            var schemas = new List<string>();

            foreach (var field in response.fields)
            {
                field.fieldName = field.fieldName.Replace(".", "").Replace("address.", "").Replace("bankDetails.", "").Replace("intermediaryBankAddress.", "intermediaryBank");
                if (field.required) schemas.Add(field.fieldName);
            }

            return schemas;
        }

        public async Task<ApiResponse> GetWalletBalanceAsync(long accountId)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "No account found for user"
                };
            }

            var wallets = await _infinitusClientIntegration.GetWalletBalanceAsync(account.Uuid);
            if (wallets is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (wallets.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch wallets"
                };
            }

            foreach (var item in wallets)
            {
                var wallet = await _walletRepository.GetAsync(accountId, item.currency);
                if (wallet is null) continue;

                wallet.Balance = (decimal)item.availableAmount;
                wallet.Network = item.network;
                wallet.Uuid = item.accountId;
                await _walletRepository.UpdateAsync(wallet);
            }

            var accountWallets = await _walletRepository.GetAsync(accountId);

            return new ApiResponse { Success = true, Message = "Successfully retrieved the balance", Data = accountWallets };
        }

        public async Task<ApiResponse> CreateGlobalAccountAsync(Account account, string currency, string country)
        {
            var wallet = await _walletRepository.GetAsync(account.Id, currency);
            if (wallet is null)
            {
                wallet = new Wallet
                {
                    AccountId = account.Id,
                    Currency = currency
                };
                bool insert = await _walletRepository.InsertAsync(wallet);
                if (!insert)
                {
                    return new ApiResponse
                    {
                        Message = "Unable to create wallet"
                    };
                }
            }

            var request = new CreateInfinitoGlobalAccountRequest
            {
                countryCode = country,
                currency = currency,
                nickName = account.Name
            };
            var response = await _infinitusClientIntegration.CreateGlobalAccountAsync(request, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create account"
                };
            }

            wallet.Uuid = response.id;
            wallet.Status = response.status.ToLower();
            await _walletRepository.UpdateAsync(wallet);

            response = await _infinitusClientIntegration.GlobalAccountAsync(response.id, account.Uuid);
            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create account"
                };
            }

            //Create virtual account
            foreach (var transferMethod in response.transferMethods)
            {
                var vAccount = await _virtualAccountRepository.GetAsync(account.Id, response.currency, transferMethod.accountNumber);
                if (vAccount is not null) continue;

                var virtualAccount = new VirtualAccount
                {
                    AccountHolderName = transferMethod.accountHolderName,
                    AccountId = account.Id,
                    AccountNumber = transferMethod.accountNumber,
                    Country = response.country,
                    Currency = response.currency,
                    Type = transferMethod.type,
                    Status = response.status.ToLower(),
                    ReferenceCode = transferMethod.referenceCode,
                    BankState = transferMethod.bankAddress.state,
                    BankCity = transferMethod.bankAddress.city,
                    BankPostalCode = transferMethod.bankAddress.postalCode,
                    BankName = transferMethod.bankName,
                    BankStreet1 = transferMethod.bankAddress.street1,
                    BankStreet2 = transferMethod.bankAddress.street2,
                    RoutingNumber = transferMethod.routingCodes.FirstOrDefault(x => x.type.Equals("routingNumber", StringComparison.Ordinal)).value,
                    SwiftCode = transferMethod.routingCodes.FirstOrDefault(x => x.type.Equals("swift", StringComparison.Ordinal))?.value
                };
                bool createAccount = await _virtualAccountRepository.InsertAsync(virtualAccount);
                if (!createAccount)
                {
                    _logService.LogInfo(nameof(InfinitusService), nameof(CreateGlobalAccountAsync), "Unable to create virtual account");
                    continue;
                }
            }

            return new ApiResponse { Success = true, Message = "Account created successfully" };
        }

        public async Task<ApiResponse> GetGlobalAccountAsync(long accountId, string currency)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "No account found for user"
                };
            }

            var wallet = await _walletRepository.GetAsync(accountId, currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "No wallet found"
                };
            }

            var response = await _infinitusClientIntegration.GlobalAccountAsync(wallet.Uuid, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create account"
                };
            }

            wallet.Uuid = response.id;
            wallet.Status = response.status.ToLower();
            await _walletRepository.UpdateAsync(wallet);

            var vaccount = await _virtualAccountRepository.GetAsync(accountId, currency);
            if (vaccount is null)
            {
                //Create virtual account
                foreach (var transferMethod in response.transferMethods)
                {
                    var virtualAccount = new VirtualAccount
                    {
                        AccountHolderName = transferMethod.accountHolderName,
                        AccountId = accountId,
                        AccountNumber = transferMethod.accountNumber,
                        Country = response.country,
                        Currency = response.currency,
                        Type = transferMethod.type,
                        Status = response.status.ToLower(),
                        ReferenceCode = transferMethod.referenceCode,
                        BankState = transferMethod.bankAddress.state,
                        BankCity = transferMethod.bankAddress.city,
                        BankPostalCode = transferMethod.bankAddress.postalCode,
                        BankName = transferMethod.bankName,
                        BankStreet1 = transferMethod.bankAddress.street1,
                        BankStreet2 = transferMethod.bankAddress.street2,
                        RoutingNumber = transferMethod.routingCodes.FirstOrDefault(x => x.type.Equals("routingNumber", StringComparison.Ordinal)).value,
                        SwiftCode = transferMethod.routingCodes.FirstOrDefault(x => x.type.Equals("swift", StringComparison.Ordinal)).value
                    };
                    bool createAccount = await _virtualAccountRepository.InsertAsync(virtualAccount);
                    if (!createAccount)
                    {
                        _logService.LogInfo(nameof(InfinitusService), nameof(GetGlobalAccountAsync), "Unable to create virtual account");
                    }
                }
            }

            return new ApiResponse { Success = true, Message = "Account created successfully" };
        }

        public async Task<ApiResponse> GetCryptoAccountAsync(long accountId, string currency)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "No account found for user"
                };
            }

            var wallet = await _walletRepository.GetAsync(accountId, currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "No wallet found"
                };
            }

            var cryptoRequest = new InfinitusCryptoAccountRequest
            {
                id = wallet.Uuid,
                assetType = wallet.Network,
                network = ""
            };

            var response = await _infinitusClientIntegration.CryptoAccountAsync(cryptoRequest, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create account"
                };
            }

            wallet.Status = response.address.ToLower();
            bool update = await _walletRepository.UpdateAsync(wallet);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update the address"
                };
            }

            return new ApiResponse { Success = true, Message = "Account created successfully" };
        }

        public Task<ApiResponse> GetAccountAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetAccountRequestAsync(Kyc kyc)
        {
            var approvalResponse = await _infinitusClientIntegration.GetAccountRequestsAsync(kyc.Uuid);
            if (approvalResponse is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (approvalResponse.items.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch account requests"
                };
            }

            var response = await _infinitusClientIntegration.GetAccountRequestAsync(approvalResponse.items[0].id);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch account request"
                };
            }

            //var accountResponse = await _infinitusClientIntegration.GetAccountAsync(response.accountId);
            //if (accountResponse is null)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Service is not reachable"
            //    };
            //}

            //if (string.IsNullOrEmpty(accountResponse.id))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to fetch account request status"
            //    };
            //}

            //if (!string.Equals(accountResponse.status, AccountStatuses.Verified, StringComparison.OrdinalIgnoreCase))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Account request yet to be approved"
            //    };
            //}

            return new ApiResponse { Message = "Created account successfully", Success = true, Data = response };
        }

        public async Task<ApiResponse> GetAccountRequestsAsync(Kyc kyc)
        {
            var response = await _infinitusClientIntegration.GetAccountRequestsAsync(kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (response.items.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch account requests"
                };
            }

            //Update the status of the accounts
            //foreach (var item in response.items)
            //{
            //    var result = await GetAccountRequestAsync(kyc, item.accountId);
            //    if (!result.Success) continue;


            //}

            return new ApiResponse { Success = true, Message = "Successfully retreieved the account requests" };
        }

        public async Task<ApiResponse> TradeAsync(CryptoTrade cryptoTrade, long accountId, string walletId)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            if (string.IsNullOrEmpty(account.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            var tradeRequest = new InfinitusTradeRequest
            {
                accountId = walletId,
                from = new InfinitusTradeFrom
                {
                    amount = (double)cryptoTrade.FromAmount,
                    asset = cryptoTrade.FromAsset,
                    network = cryptoTrade.FromNetwork
                },
                to = new InfinitusTradeTo
                {
                    asset = cryptoTrade.ToAsset,
                    network = cryptoTrade.ToNetwork
                }
            };
            var response = await _infinitusClientIntegration.TradeAsync(tradeRequest, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create trade"
                };
            }
            cryptoTrade.Reference = response.id;
            cryptoTrade.Status = TransactionStatuses.Successful;
            bool update = await _cryptoTradeRepository.UpdateAsync(cryptoTrade);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to create payout"
                };
            }

            return new ApiResponse { Success = true, Message = "Trade processed successfully" };
        }

        public async Task<ApiResponse> CreateTransferAsync(Transfer transfer, string recipientUuid)
        {
            var account = await _accountRepository.DetailsAsync(transfer.AccountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            var wallet = await _walletRepository.GetAsync(transfer.AccountId, transfer.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve wallet"
                };
            }

            if (string.IsNullOrEmpty(wallet.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve Wallet ID"
                };
            }

            var payoutRequest = new CreateInfinitusPayoutRequest
            {
                paymentAmount = (double)transfer.Amount,
                paymentCurrency = transfer.Currency,
                reason = transfer.Reason.ToLower().Replace(" ", "_"),
                paymentMethod = transfer.PaymentMethod.ToUpper(),
                recipientId = recipientUuid,
                reference = transfer.Reference,
                sourceAccountId = wallet.Uuid,
                sourceCurrency = transfer.Currency,
                swiftChargeOption = InfinitusPayChargeTypes.Shared
            };
            var response = await _infinitusClientIntegration.PayoutAsync(payoutRequest, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.status))
            {
                return new ApiResponse
                {
                    Message = "Unable to create payout"
                };
            }

            //Update the Uuid column
            transfer.Status = TransactionStatuses.Processing;
            transfer.GatewayReference = response.paymentId;
            transfer.GatewayRequest = JsonConvert.SerializeObject(payoutRequest);
            transfer.GatewayResponse = JsonConvert.SerializeObject(response);
            bool update = await _payoutRepository.UpdateAsync(transfer);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update payout"
                };
            }

            return new ApiResponse { Success = true, Message = "Payout created successfully" };
        }

        public async Task<ApiResponse> TransferAsync(Transfer transfer, string accountUuId)
        {
            var response = await _infinitusClientIntegration.GetPayoutAsync(transfer.GatewayReference, accountUuId);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.status))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve transfer"
                };
            }

            if (string.Equals(response.status, InfinitusTransactionStatuses.Failed, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = "Failed"
                };
            }

            if (!string.Equals(response.status, InfinitusTransactionStatuses.Completed, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Transfer is under processing"
                };
            }

            //Update the Uuid column
            transfer.Status = string.Equals(response.status, InfinitusTransactionStatuses.Completed, StringComparison.OrdinalIgnoreCase) ? TransactionStatuses.Successful : TransactionStatuses.Processing;
            transfer.QueryStatusResponse = JsonConvert.SerializeObject(response);
            bool update = await _payoutRepository.UpdateAsync(transfer);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update transfer"
                };
            }

            return new ApiResponse { Success = true, Message = "Successful" };
        }

        public async Task<ApiResponse> SubmitClientAsync(Kyc kyc, List<string> providers)
        {
            if (string.IsNullOrEmpty(kyc.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve kyc ID"
                };
            }

            var clientRequest = new SubmitInfinitusClientRequest
            {
                providers = providers,
                useHostedFlow = false
            };
            var response = await _infinitusClientIntegration.SubmitClientAsync(clientRequest, kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (response.SubmitInfinitusClients is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to create profile"
                };
            }

            if (string.IsNullOrEmpty(response.SubmitInfinitusClients[0].Id))
            {
                return new ApiResponse
                {
                    Message = "Unable to create profile"
                };
            }

            //Update the Uuid column
            //payoutRecipient.Uuid = response.id;
            //bool update = await _recipientRepository.UpdateAsync(payoutRecipient);
            //if (!update)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to update payout Recipient"
            //    };
            //}

            return new ApiResponse { Success = true, Message = "Submitted application successfully" };
        }

        public async Task<ApiResponse> UpdateBusinessClientAsync(Business business, long userId)
        {
            var infinitusCountriesOfOperation = new InfintusCountriesOfOperation();

            var user = await _userRepository.GetDetailsByUserIdAsync(userId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve user details"
                };
            }

            var kyc = await _kycRepository.DetailsByUserAsync(userId);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve kyc details"
                };
            }

            if (string.IsNullOrEmpty(kyc.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve kyc ID"
                };
            }

            string[] currencies = business.CurrencyNeeded.Split(',');

            var currencyNeeded = currencies.ToList();

            string[] countryList = business.CountriesOfOperation.Split(',');

            var countries = await _countryRepository.GetCountriesAsync(countryList.ToList());
            foreach (var country in countries)
            {
                if (string.Equals(country.Region, Regions.NorthAmerica, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.northAmerica.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.SouthAmerica, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.southAmerica.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.Caribbean, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.caribbean.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.Apac, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.apac.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.MiddleEast, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.middleEast.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.RestOfEurope, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.restOfEurope.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.Europe, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.europe.Add(country.Code);
                }

                if (string.Equals(country.Region, Regions.Africa, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.africa.Add(country.Code);
                }
                if (string.Equals(country.Region, Regions.Eeaefta, StringComparison.OrdinalIgnoreCase))
                {
                    infinitusCountriesOfOperation.eeaEfta.Add(country.Code);
                }
            }

            if (!string.Equals(kyc.Country, Countries.United_Kingdom, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(business.TaxId) && !business.TaxId.Contains('-'))
            {
                business.TaxId = $"{business.TaxId[..2]}-{business.TaxId[2..]}";
                // business.TaxId = $"{business.TaxId[..3]}-{business.TaxId[3..5]}-{business.TaxId[5..]}";
            }

            var clientRequest = new UpdateBusinessInfinitusClientRequest
            {
                onboardingDetails = new BusinessInfintusOnboardingDetails
                {
                    businessCountry = business.Country,
                    businessAddress = new InfintusBusinessAddress
                    {
                        city = business.City,
                        country = business.Country,
                        line1 = business.Address1,
                        line2 = business.Address2,
                        postalCode = business.PostalCode,
                        state = business.State.Substring(2)
                    },
                    businessInfo = new InfintusBusinessInfo
                    {
                        website = business.Website,
                        description = business.Description,
                        identifier = business.Identifier,
                        incorporationDate = business.IncorporationDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                        name = business.Name,
                        type = business.Type,
                        taxId = business.TaxId,
                        tradeName = business.TradeName,
                        monthlyConversionVolumeDigitalAssets = business.MonthlyConversionVolumeDigitalAssets,
                        monthlyConversionVolumeFiat = business.MonthlyConversionVolumeFiat,
                        monthlySwiftVolume = business.MonthlySwiftVolume,
                        monthlyLocalPaymentsVolume = business.MonthlyLocalPaymentVolume,
                        monthlyRevenue = business.MonthlyRevenue,
                        currencyNeeded = currencyNeeded
                    },
                    mailingAddress = new InfintusMailingAddress
                    {
                        city = business.MailingCity,
                        country = business.MailingCountry,
                        line1 = business.MailingAddress1,
                        line2 = business.MailingAddress2,
                        postalCode = business.MailingPostalCode,
                        state = business.MailingState.Substring(2)
                    },
                    generalInfo = new InfintusGeneralInfo
                    {
                        additionalIndustry = business.AdditionalIndustry,
                        contactEmail = user.EmailAddress,
                        contactFirstName = user.FirstName,
                        contactLastName = user.LastName,
                        contactPhone = user.PhoneNumber,
                        mainIndustry = business.MainIndustry,
                        naics = business.Naics,
                        naicsDescription = business.NaicsDescription
                    },
                    countriesOfOperation = infinitusCountriesOfOperation,
                    regionsOfOperation = new InfintusRegionsOfOperation
                    {
                        africa = infinitusCountriesOfOperation.africa.Count > 0,
                        apac = infinitusCountriesOfOperation.apac.Count > 0,
                        caribbean = infinitusCountriesOfOperation.caribbean.Count > 0,
                        eeaEfta = infinitusCountriesOfOperation.eeaEfta.Count > 0,
                        europe = infinitusCountriesOfOperation.europe.Count > 0,
                        middleEast = infinitusCountriesOfOperation.middleEast.Count > 0,
                        northAmerica = infinitusCountriesOfOperation.northAmerica.Count > 0,
                        restOfEurope = infinitusCountriesOfOperation.restOfEurope.Count > 0,
                        southAmerica = infinitusCountriesOfOperation.southAmerica.Count > 0
                    }
                }
            };
            var response = await _infinitusClientIntegration.UpdateBusinessClientAsync(clientRequest, kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            //Update the Uuid column
            business.DataUploaded = true;
            bool update = await _businessRepository.UpdateAsync(business);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update business profile"
                };
            }

            return new ApiResponse { Success = true, Message = "Updated profile successfully" };
        }

        public async Task<ApiResponse> UpdatePersonalClientAsync(User user)
        {
            var kyc = await _kycRepository.DetailsByUserAsync(user.Id);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve kyc details"
                };
            }

            if (string.IsNullOrEmpty(kyc.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve kyc ID"
                };
            }

            if (!string.Equals(kyc.Country, Countries.United_Kingdom, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(kyc.TaxNumber) && !kyc.TaxNumber.Contains('-'))
            {
                kyc.TaxNumber = $"{kyc.TaxNumber[..2]}-{kyc.TaxNumber[2..]}";
            }

            var clientRequest = new UpdatePersonalInfinitusClientRequest
            {
                address = new InfinitusAddress
                {
                    city = kyc.City,
                    country = kyc.Country,
                    line1 = kyc.Address1,
                    line2 = kyc.Address2,
                    postalCode = kyc.PostalCode,
                    state = kyc.ResidentialState.Substring(2)
                },
                annualIncome = kyc.AnnualIncome,
                citizenship = kyc.Citizenship,
                dateOfBirth = kyc.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                employer = kyc.Employer,
                email = user.EmailAddress,
                employerCountry = kyc.EmployerCountry,
                employerState = kyc.EmployerState.Substring(2),
                employmentStatus = kyc.EmploymentStatus,
                firstName = user.FirstName,
                idDocument = new InfinitusIdDocument
                {
                    expirationDate = kyc.IdentityExpiryDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    issueDate = kyc.IdentityIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    number = kyc.IdentityNumber,
                    type = kyc.IdentityType
                },
                incomeCountry = kyc.IncomeCountry,
                incomeSource = kyc.IncomeSource,
                incomeState = kyc.IncomeState.Substring(2),
                industry = kyc.Industry,
                lastName = user.LastName,
                occupation = kyc.Occupation,
                occupationDescription = kyc.OccupationDescription,
                phone = user.PhoneNumber,
                sourceOfFunds = kyc.SourceOfFund,
                taxNumber = kyc.TaxNumber,
                wealthSource = kyc.WealthSource,
                wealthSourceDescription = kyc.WealthSourceDescription
            };
            var response = await _infinitusClientIntegration.UpdatePersonalClientAsync(clientRequest, kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            //Update the Uuid column
            kyc.DataUploaded = true;
            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            return new ApiResponse { Success = true, Message = "Profile updated successfully" };
        }

        public async Task<ApiResponse> UpdateRecipientAsync(TransferRecipient recipient)
        {
            var account = await _accountRepository.DetailsAsync(recipient.AccountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account"
                };
            }

            if (string.IsNullOrEmpty(account.Uuid))
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve account ID"
                };
            }

            var clientRequest = new CreateInfinitusRecipientRequest
            {
                address = new InfinitusRecipientAddress
                {
                    city = recipient.City,
                    countryCode = recipient.Country,
                    streetAddress = recipient.StreetAddress,
                    postCode = recipient.PostCode,
                    state = recipient.State
                },
                intermediaryBankAddress = new InfinitusRecipientIntermediaryBankAddress
                {
                    city = recipient.IntermediaryCity,
                    country = recipient.IntermediaryBankCountry,
                    postalCode = recipient.IntermediaryPostalCode,
                    state = recipient.IntermediaryState,
                    street1 = recipient.IntermediaryStreet1,
                    street2 = recipient.IntermediaryStreet2
                },
                recipientAccountType = recipient.Type,
                paymentMethod = recipient.PaymentMethod,
                lastName = "",
                firstName = "",
                companyName = recipient.AccountName,
                intermediaryRoutingCode = recipient.IntermediaryRoutingCode,
                intermediaryBankName = recipient.IntermediaryBankName,
                internationalBankName = recipient.InternationalBankName,
                bankDetails = new InfinitusRecipientBankDetails
                {
                    accountCurrency = recipient.Currency,
                    accountName = recipient.AccountName,
                    accountNumber = recipient.AccountNumber,
                    accountRoutingType1 = recipient.AccountRoutingType,
                    bankAccountType = recipient.BankAccountType,
                    bankBranch = recipient.BankBranch,
                    bankCity = recipient.BankCity,
                    bankCountryCode = recipient.BankCountry,
                    bankName = recipient.BankName,
                    bankPostalCode = recipient.BankPostalCode,
                    bankState = recipient.BankState,
                    bankStreetAddress = recipient.BankStreetAddress,
                    iban = recipient.AccountNumber,
                    routingNumber = recipient.RoutingNumber,
                    swiftCode = recipient.SwiftCode
                },
                additionalInfo = new InfinitusRecipientAdditionalInfo
                {
                    businessPhoneNumber = recipient.PhoneNumber,
                    dateOfBirth = recipient.DateOfBirth == null ? null : recipient.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    email = recipient.Email,
                    personalMobileNumber = recipient.PhoneNumber
                }
            };
            var response = await _infinitusClientIntegration.UpdateRecipientAsync(clientRequest, recipient.Uuid, account.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to update recipient"
                };
            }

            return new ApiResponse { Success = true, Message = "Recipient updated successfully" };
        }

        public async Task<ApiResponse> UpdateRepresentativeAsync(User user)
        {
            var kyc = await _kycRepository.DetailsByUserAsync(user.Id);
            if (kyc is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to retrieve KYC"
                };
            }
            var businessTeam = await _businessTeamRepository.GetByUserIdAsync(user.Id);
            if (businessTeam is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch details of teams"
                };
            }

            var clientRequest = new RepresentativeInfinitusRequest
            {
                address = new InfinitusAddress
                {
                    city = kyc.City,
                    country = kyc.Country,
                    line1 = kyc.Address1,
                    line2 = kyc.Address2,
                    postalCode = kyc.PostalCode,
                    state = kyc.ResidentialState
                },
                wealthSourceDescription = kyc.WealthSourceDescription,
                annualIncome = kyc.AnnualIncome,
                citizenship = kyc.Citizenship,
                dateOfBirth = kyc.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                email = user.EmailAddress,
                employer = kyc.Employer,
                employerCountry = kyc.EmployerCountry,
                employerState = kyc.EmployerState,
                employmentStatus = kyc.EmploymentStatus,
                firstName = user.FirstName,
                idDocument = new InfinitusIdDocument
                {
                    number = kyc.IdentityNumber,
                    type = kyc.IdentityType,
                    expirationDate = kyc.IdentityExpiryDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    issueDate = kyc.IdentityIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd")
                },
                incomeCountry = kyc.IncomeCountry,
                incomeSource = kyc.IncomeSource,
                incomeState = kyc.IncomeState,
                isSigner = kyc.IsSigner,
                industry = kyc.Industry,
                lastName = user.LastName,
                occupation = kyc.Occupation,
                occupationDescription = kyc.OccupationDescription,
                ownershipPercentage = kyc.OwnershipPercentage,
                phone = user.PhoneNumber,
                role = kyc.Role,
                sourceOfFunds = kyc.WealthSource,
                taxNumber = kyc.TaxNumber,
                title = kyc.Title,
                wealthSource = kyc.WealthSource
            };
            var response = await _infinitusClientIntegration.UpdateRepresentativeAsync(clientRequest, kyc.Uuid, kyc.Uuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to update profile"
                };
            }

            //Update the Uuid column
            kyc.Uuid = response.id;
            bool update = await _kycRepository.UpdateAsync(kyc);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update representative"
                };
            }

            return new ApiResponse { Success = true, Message = "Profile created successfully" };
        }

        public async Task<ApiResponse> GetTransactionsAsync(string accountUuid, string type)
        {
            //var account = await _accountRepository.DetailsAsync(accountId);
            //if (account is null)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to retrieve account"
            //    };
            //}

            //if (string.IsNullOrEmpty(account.Uuid))
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to retrieve account ID"
            //    };
            //}

            var request = new GetInfinitusTransactionRequest
            {
                type = type
            };

            var response = await _infinitusClientIntegration.GetTransactionsAsync(request, accountUuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (response.items.Count == 0)
            {
                return new ApiResponse
                {
                    Message = "No transaction found"
                };
            }

            return new ApiResponse { Success = true, Data = response.items, Message = "Successfully retrieved" };
        }

        public async Task<ApiResponse> CreateConversionAsync(TradeFxRequest request, string accountId)
        {
            var tradeRequest = new CreateConversionInfinitusRequest
            {
                // buyAmount = (double)request.ToAmount,
                // buyCurrency = request.ToCurrency,
                //  sellAmount = (double)request.ToAmount,
                // sellCurrency = request.ToCurrency
            };

            var response = await _infinitusClientIntegration.CreateConversionAsync(tradeRequest, accountId);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to process request"
                };
            }

            return new ApiResponse { Success = true, Data = response, Message = "Fx request has been processed successfully" };
        }

        public async Task<ApiResponse> GetConversionAsync(string conversionId, string accountId)
        {
            var response = await _infinitusClientIntegration.GetConversionAsync(conversionId, accountId);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "Unable to process request"
                };
            }

            return new ApiResponse { Success = true, Data = response, Message = "Fx trade details Successfully retrieved" };
        }
        public Task<ApiResponse> DeleteRecipientAsync(string recipientId, string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteRepresentativeAsync(string clientId, string representativeId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> DepositAsync(string accountUuid, string depositId)
        {
            var response = await _infinitusClientIntegration.DepositAsync(depositId, accountUuid);
            if (response is null)
            {
                return new ApiResponse
                {
                    Message = "Service is not reachable"
                };
            }

            if (string.IsNullOrEmpty(response.id))
            {
                return new ApiResponse
                {
                    Message = "No transaction found"
                };
            }

            return new ApiResponse { Success = true, Data = response, Message = "Successfully retrieved" };
        }

        public Task<ApiResponse> GetRepresentativeAsync(string clientId, string representativeId)
        {
            throw new NotImplementedException();
        }

        public async Task TestAsync()
        {
            var request = new CreateInfinitusWebhookRequest
            {
                eventTypes = new List<string>() { "DEPOSIT" },
                secret = "RWXJE@LcFqJ4LXFlDmx1!VVHnf4dmA0Y!gWGlbSvEuHbf0wmeMfjusttest",
                url = "https://gabtrans-d9hhb8gyf8c9eed9.uksouth-01.azurewebsites.net/api/v1/webhook/infinitus/deposit"
            };
            var response = await _infinitusClientIntegration.CreateWebhookAsync(request);
            if (response is null || string.IsNullOrEmpty(response.id)) return;

            var result = response;

        }

        public async Task Test222Async()
        {
            string accountId = "bef7dc74-c182-4523-b4a3-360021cea02d";

            var request = new SimulateInfinitusTopupRequest
            {
                amount = "1000",
                counterPartyName = "James Garner",
                globalAccountId = "7026220a-a255-401e-85c0-65abed0785a1",//Wallet ID
                reference = "202602200341004"
            };
            var response = await _infinitusClientIntegration.SimulateTopupAsync(request, accountId);
            if (response is null || string.IsNullOrEmpty(response.id)) return;

            var result = response;

        }
    }
}
