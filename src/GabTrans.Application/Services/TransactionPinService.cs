using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using System;


namespace GabTrans.Application.Services;

public class TransactionPinService(IKycRepository kycRepository, IUserRepository userRepository, ITransactionPinRepository transactionPinRepository, IEncryptionService encryptionService) : ITransactionPinService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IKycRepository _kycRepository = kycRepository;
    private readonly ITransactionPinRepository _transactionPinRepository = transactionPinRepository;
    private readonly IEncryptionService _encryptionService = encryptionService;

    public async Task<ApiResponse> CreateAsync(AuthorizationPinRequest request, long userId)
    {
        var kyc = await _kycRepository.DetailsByUserAsync(userId);
        if (kyc is null)
        {
            return new ApiResponse
            {
                Message = "User details not found"
            };
        }

        var details = await _transactionPinRepository.GetAsync(userId);
        if (details is not null)
        {
            return new ApiResponse
            {
                Message = "User already have a pin"
            };
        }

        var authorizationPin = new TransactionPin
        {
            UserId = userId,
            NewPin = _encryptionService.EncryptPassword(request.PinConfirmation),
            Trials = 0,
             CreatedAt=DateTime.Now
        };

        bool insert = await _transactionPinRepository.InsertAsync(authorizationPin);
        if (!insert)
        {
            return new ApiResponse
            {
                Message = "Unable to setup transaction pin"
            };
        }

        kyc.HasPin = true;
        bool update = await _kycRepository.UpdateAsync(kyc);
        if (!update)
        {
            return new ApiResponse
            {
                Message = "Unable to update Transaction Pin"
            };
        }

        return new ApiResponse { Success = true, Message = "Pin successfully created." };
    }

    public async Task<ApiResponse> UpdateAsync(UpdateAuthorizationPinRequest request, long userId)
    {
        var authorization = await _transactionPinRepository.GetAsync(userId);
        if (authorization == null)
        {
            return new ApiResponse
            {
                Message = " User details not found"
            };
        }

        if (string.IsNullOrEmpty(request.CurrentPin))
        {
            return new ApiResponse
            {
                Message = "Kindly provide a valid Pin"
            };
        }

        bool validatePin = _encryptionService.VerifyHash(request.CurrentPin, authorization.NewPin);
        if (!validatePin)
        {
            return new ApiResponse { Message = "Invalid current pin" };
        }

        if (string.IsNullOrEmpty(request.Pin))
        {
            return new ApiResponse
            {
                Message = "Invalid pin"
            };
        }

        if (request.Pin != request.PinConfirmation)
        {
            return new ApiResponse { Message = "Pin must match pin confirmation" };
        }

        var user = await _userRepository.GetDetailsByUserIdAsync(userId);
        if (user is null)
        {
            return new ApiResponse
            {
                Message = "user record does not exist"
            };
        }

        authorization.OldPin = authorization.NewPin;
        authorization.NewPin = _encryptionService.EncryptPassword(request.PinConfirmation);
        bool updatePin = await _transactionPinRepository.UpdateAsync(authorization);

        if (!updatePin)
        {
            return new ApiResponse
            {
                Message = "Unable to update transaction pin"
            };
        }

        return new ApiResponse { Message = "Pin updated successfully", Success = true };
    }

    public async Task<ApiResponse> ValidateAsync(long userId, string pin)
    {
        var details = await _transactionPinRepository.GetAsync(userId);
        if (details == null)
        {
            return new ApiResponse
            {
                Message = "pin details not found"
            };
        }

        if (details.Trials > 2)
        {
            return new ApiResponse
            {
                Message = "Your profile has been locked, Kindly change your pin"
            };
        }

        if (!_encryptionService.VerifyHash(pin, details.NewPin))
        {
            details.Trials = details.Trials + 1;
            await _transactionPinRepository.UpdateAsync(details);
        }



        return new ApiResponse { Message = "Successfully validated", Success = true };
    }
}