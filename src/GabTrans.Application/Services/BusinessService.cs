using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Newtonsoft.Json;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace GabTrans.Application.Services
{
    public class BusinessService(IEmailNotificationService emailService, IKycRepository kycRepository, IAccountRepository accountRepository, ICountryRepository countryRepository, IBusinessRepository businessRepository, IAccountRequestRepository kycRequestRepository, IBusinessTeamRepository businessTeamRepository) : IBusinessService
    {
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IKycRepository _kycRepository = kycRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly IAccountRequestRepository _kycRequestRepository = kycRequestRepository;
        private readonly IBusinessTeamRepository _businessTeamRepository = businessTeamRepository;

        public async Task<BusinessObject> DetailsByAccountIdAsync(long accountId)
        {
            return await _businessRepository.DetailsByAccountIdAsync(accountId);
        }

        public Task<ApiResponse> UpdateAsync(UpdateBusinessRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CreateAsync(long userId, string name)
        {
            var details = await _businessRepository.GetByUserAsync(userId);
            if (details is not null)
            {
                return new ApiResponse
                {
                    Message = "User already created an existing business"
                };
            }

            var business = new Business
            {
                UserId = userId,
                Name = name,
                Status = AccountStatuses.Active
            };

            bool insert = await _businessRepository.InsertAsync(business);
            if (!insert)
            {
                return new ApiResponse
                {
                    Message = "Unable to create business"
                };
            }
            return new ApiResponse { Success = true };
        }

        //public async Task<ApiResponse> UpdateAddressAsync(UpdateBusinessAddressRequest request, long userId)
        //{
        //    var business = await _businessRepository.GetByUserAsync(userId);
        //    if (business is null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Business does not exist"
        //        };
        //    }

        //    var kyc = await _kycRepository.DetailsByUserAsync(userId);
        //    if (kyc == null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Kyc detail not found"
        //        };
        //    }

        //    business.Address1 = request.Address1;
        //    business.Address2 = request.Address2;
        //    business.City = request.City;
        //    business.State = request.State;
        //    business.Country = request.Country;
        //    business.PostalCode = request.PostalCode;
        //    business.MailingCity = request.MailingCity;
        //    business.MailingCountry = request.MailingCountry;
        //    business.MailingAddress1 = request.MailingAddress1;
        //    business.MailingAddress2 = request.MailingAddress2;
        //    business.MailingPostalCode = request.MailingPostalCode;
        //    business.MailingState = request.MailingState;
        //    business.UpdateAddress = true;

        //    bool update = await _businessRepository.UpdateAsync(business);
        //    if (!update)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Unable to update address"
        //        };
        //    }

        //    if (business.UpdateAddress && business.UpdateDocument && business.UpdateInformation)
        //    {
        //        kyc.Status = KycStatuses.Submitted;
        //        update = await _kycRepository.UpdateAsync(kyc);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC"
        //            };
        //        }

        //        update = await _kycRequestRepository.UpdateAsync(business.UserId, KycStatuses.Submitted);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC approval request"
        //            };
        //        }

        //        await _emailService.AccountRequestAsync(business.Name, AccountTypes.Business, business.CreatedAt);
        //    }

        //    return new ApiResponse { Message = "Successfully updated the Address information", Success = true };
        //}

        //public async Task<ApiResponse> UpdateDocumentAsync(UpdateBusinessDocumentRequest request, long userId)
        //{
        //    var business = await _businessRepository.GetByUserAsync(userId);
        //    if (business is null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Business does not exist"
        //        };
        //    }

        //    var kyc = await _kycRepository.DetailsByUserAsync(userId);
        //    if (kyc == null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Kyc detail not found"
        //        };
        //    }

        //    business.BankStatement = request.BankStatement;
        //    business.FormationDocument = request.FormationDocument;
        //    business.ProofOfOwnership = request.ProofOfOwnership;
        //    business.ProofOfRegistration = request.ProofOfRegistration;
        //    business.TaxDocument = request.TaxDocument;
        //    business.Agreement = request.Agreement;
        //    business.UpdateDocument = true;

        //    bool update = await _businessRepository.UpdateAsync(business);
        //    if (!update)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Unable to update documents"
        //        };
        //    }

        //    if (business.UpdateAddress && business.UpdateDocument && business.UpdateInformation)
        //    {
        //        kyc.Status = KycStatuses.Submitted;
        //        update = await _kycRepository.UpdateAsync(kyc);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC"
        //            };
        //        }

        //        update = await _kycRequestRepository.UpdateAsync(business.UserId, KycStatuses.Submitted);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC approval request"
        //            };
        //        }

        //        await _emailService.AccountRequestAsync(business.Name, AccountTypes.Business, business.CreatedAt);
        //    }

        //    return new ApiResponse { Message = "Business details updated successfully", Success = true };
        //}

        //public async Task<ApiResponse> UpdateBusinessInformationAsync(UpdateBusinessInformationRequest request, long userId)
        //{
        //    var business = await _businessRepository.GetByUserAsync(userId);
        //    if (business is null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Business does not exist"
        //        };
        //    }

        //    var kyc = await _kycRepository.DetailsByUserAsync(userId);
        //    if (kyc == null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Kyc detail not found"
        //        };
        //    }

        //    business.CurrencyNeeded = request.CurrenciesNeeded;
        //    business.Description = request.Description;
        //    business.Identifier = request.Identifier;
        //    business.TaxId = kyc.TaxNumber;
        //    business.IncorporationDate = request.IncorporationDate;
        //    business.MonthlyConversionVolumeDigitalAssets = request.MonthlyConversionVolumeDigitalAssets;
        //    business.MonthlyConversionVolumeFiat = request.MonthlyConversionVolumeFiat;
        //    business.MonthlyLocalPaymentVolume = request.MonthlyLocalPaymentVolume;
        //    business.MonthlyRevenue = request.MonthlyRevenue;
        //    business.MonthlySwiftVolume = request.MonthlySWIFTVolume;
        //    business.TradeName = request.TradeName;
        //    business.Type = request.Type;
        //    business.Website = request.Website;
        //    business.CountriesOfOperation = request.CountriesOfOperation;
        //    business.MainIndustry = request.MainIndustry;
        //    business.Naics = request.NAICS;
        //    business.NaicsDescription = request.NAICSDescription;
        //    business.UpdateInformation = true;

        //    bool update = await _businessRepository.UpdateAsync(business);
        //    if (!update)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Unable to update address"
        //        };
        //    }

        //    if (business.UpdateAddress && business.UpdateDocument && business.UpdateInformation)
        //    {
        //        kyc.Status = KycStatuses.Submitted;
        //        update = await _kycRepository.UpdateAsync(kyc);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC"
        //            };
        //        }

        //        update = await _kycRequestRepository.UpdateAsync(business.UserId, KycStatuses.Submitted);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC approval request"
        //            };
        //        }

        //        await _emailService.AccountRequestAsync(business.Name, AccountTypes.Business, business.CreatedAt);
        //    }

        //    return new ApiResponse { Message = "Updated the Business information successfully", Success = true };
        //}

        //public async Task<ApiResponse> UpdateGeneralInformationAsync(UpdateGeneralInformationRequest request, long userId)
        //{
        //    var business = await _businessRepository.GetByUserAsync(userId);
        //    if (business is null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Business does not exist"
        //        };
        //    }

        //    var kyc = await _kycRepository.DetailsByUserAsync(userId);
        //    if (kyc == null)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Kyc detail not found"
        //        };
        //    }

        //    //business.AdditionalIndustry = request.AdditionalIndustry;
        //    // business. = request.ContactEmail;
        //    // business. = request.ContactFirstName;
        //    // business.Country = request.ContactLastName;
        //    // business. = request.ContactPhone;
        //    business.MainIndustry = request.MainIndustry;
        //    business.Naics = request.NAICS;
        //    business.NaicsDescription = request.NAICSDescription;
        //    //business.UpdateGeneralInformation = true;

        //    bool update = await _businessRepository.UpdateAsync(business);
        //    if (!update)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = "Unable to update the general information"
        //        };
        //    }

        //    if (business.UpdateAddress && business.UpdateDocument && business.UpdateInformation)
        //    {
        //        kyc.Status = KycStatuses.Submitted;
        //        update = await _kycRepository.UpdateAsync(kyc);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC"
        //            };
        //        }

        //        update = await _kycRequestRepository.UpdateAsync(business.UserId, KycStatuses.Submitted);
        //        if (!update)
        //        {
        //            return new ApiResponse
        //            {
        //                Message = "Unable to update KYC approval request"
        //            };
        //        }

        //        await _emailService.AccountRequestAsync(business.Name, AccountTypes.Business, business.CreatedAt);
        //    }

        //    return new ApiResponse { Message = "Updated the General information successfully", Success = true };
        //}

        // public async Task<ApiResponse> UpdateCountryOfOperationAsync(UpdateCountryOperationRequest request, long businessId)
        // {
        //     var business = await _businessRepository.DetailsByIdAsync(businessId);
        //     if (business is null)
        //     {
        //         return new ApiResponse
        //         {
        //             Message = "Business does not exist"
        //         };
        //     }

        //     string countriesOfOperation = string.Empty;
        //     if (request.Countries.Count > 0)
        //     {
        //         countriesOfOperation = string.Join(",", request.Countries);
        //     }

        //     business.CountriesOfOperation = countriesOfOperation;
        //     bool update = await _businessRepository.UpdateAsync(business);
        //     if (!update)
        //     {
        //         return new ApiResponse
        //         {
        //             Message = "Unable to update address"
        //         };
        //     }

        //     return new ApiResponse { Message = "Updated the Country of operation successfully", Success = true };
        // }

        // public async Task<ApiResponse> UpdateMailingAddressAsync(UpdateMailingAddressRequest request, long businessId)
        // {
        //     var business = await _businessRepository.DetailsByIdAsync(businessId);
        //     if (business is null)
        //     {
        //         return new ApiResponse
        //         {
        //             Message = "Business does not exist"
        //         };
        //     }

        //     business.MailingCity = request.City;
        //     business.MailingCountry = request.Country;
        //     business.MailingAddress1 = request.Line1;
        //     business.MailingAddress2 = request.Line2;
        //     business.MailingPostalCode = request.PostalCode;
        //     business.State = request.State;

        //     bool update = await _businessRepository.UpdateAsync(business);
        //     if (!update)
        //     {
        //         return new ApiResponse
        //         {
        //             Message = "Unable to update address"
        //         };
        //     }

        //     return new ApiResponse { Message = "Updated the mailing Address successfully", Success = true };
        // }
    }
}
