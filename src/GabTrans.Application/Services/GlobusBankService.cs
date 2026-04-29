using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.GlobusBank;
using GabTrans.Application.Interfaces.Integrations;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;

namespace GabTrans.Application.Services;

public class GlobusBankService(IAccountRepository accountRepository, ITransferRepository transferRepository, IVirtualAccountRepository virtualAccountRepository, IGlobusBankClientIntegration globusBankClientIntegration) : IGlobusBankService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly IVirtualAccountRepository _virtualAccountRepository = virtualAccountRepository;
    private readonly IGlobusBankClientIntegration _globusBankClientIntegration = globusBankClientIntegration;

    public async Task<ApiResponse> BankListAsync()
    {
        var bank = await _globusBankClientIntegration.BanksAsync();
        if (bank is null)
        {
            return new ApiResponse
            {
                Message = "Bank not found"
            };
        }

        return new ApiResponse { Data = bank.result, Message = "Successful", Success = true };
    }

    public async Task<ApiResponse> GetCitiesAsync(long stateid)
    {
        var city = await _globusBankClientIntegration.GetCitiesAsync(stateid);
        if (city is null)
        {
            return new ApiResponse
            {
                Message = "Bank not found"
            };
        }

        return new ApiResponse { Data = city.data, Message = "Successful", Success = true };
    }

    public async Task<ApiResponse> GetStatesAsync()
    {
        var state = await _globusBankClientIntegration.GetAllStateAsync();
        if (state is null)
        {
            return new ApiResponse
            {
                Message = "Bank not found"
            };
        }

        return new ApiResponse { Data = state.data, Message = "Successful", Success = true };
    }

    public async Task<GlobusAccountBalanceResponse> GetBalanceAsync(string accountNo)
    {
        return await _globusBankClientIntegration.AccountBalanceAsync(accountNo);
    }

    public async Task<ApiResponse> GetNameEnquiryAsync(string accountno, string bankcode)
    {
        var lookupResponse = await _globusBankClientIntegration.NameEnquiryAsync(accountno, bankcode);
        if (lookupResponse is null || string.IsNullOrEmpty(lookupResponse.responsecode))
        {
            return new ApiResponse
            {
                Message = "Account details not found"
            };
        }

        if (lookupResponse.responsecode!="00")
        {
            return new ApiResponse
            {
                Message = "Account details not found"
            };
        }

        return new ApiResponse { Data = lookupResponse.result.accountname, Message = "Successful", Success = true };
    }

    public async Task<ApiResponse> TransactionQueryAsync(Transfer transfer)
    {
        var response = await _globusBankClientIntegration.TransactionQueryAsync(transfer.Reference);
        if (response is null || string.IsNullOrEmpty(response.statusCode))
        {
            return new ApiResponse
            {
                Message = "Service is not reachable"
            };
        }

        if (response.statusCode == "23" || response.statusCode == "24")
        {
            transfer.Status = TransactionStatuses.Processing;
            transfer.QueryStatusResponse = JsonConvert.SerializeObject(response);
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse
            {
                Message = "Under processing"
            };
        }

        if (response.statusCode == "01" || response.statusCode == "02")
        {
            transfer.Status = TransactionStatuses.Processing;
            transfer.QueryStatusResponse = JsonConvert.SerializeObject(response);
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse
            {
                Message = "Under processing"
            };
        }

        if (response.statusCode == "00")
        {
            transfer.Status = TransactionStatuses.Successful;
            transfer.GatewayReference = response.sessionId;
            transfer.QueryStatusResponse = JsonConvert.SerializeObject(response);
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse { Data = response, Message = "Successful", Success = true };
        }

        if (response.statusCode != "00")
        {
            transfer.Status = TransactionStatuses.Failed;
            transfer.QueryStatusResponse = JsonConvert.SerializeObject(response);
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse { Data = response, Message = TransactionStatuses.Failed, Success = true };
        }

        return new ApiResponse { Data = response, Message = "Successful", Success = true };
    }

    public async Task<GlobusSingleTransferResponse> SingleTransferAsync(GlobusSingleTransferRequest request)
    {
        return await _globusBankClientIntegration.SingleTransferAsync(request);
    }

    public async Task<ApiResponse> DoTransferAsync(DoTransferRequest request)
    {
        var transfer = request.Transfer;
        var receipient = request.TransferRecipient;

        var account = await _accountRepository.DetailsAsync(transfer.AccountId);
        if(account is null)
        {
            return new ApiResponse
            {
                Message = "Account not found"
            };
        }

        var accBalance = await _globusBankClientIntegration.AccountBalanceAsync(StaticData.GlobusLinkedAccount);
        if (accBalance is null || string.IsNullOrEmpty(accBalance.responsecode))
        {
            return new ApiResponse
            {
                Message = "Service not reachable"
            };
        }

        if (accBalance.result.balance < (double)transfer.Amount)
        {
            return new ApiResponse
            {
                Message = "Unable to process transfer at the moment"
            };
        }

        var nameEnquiry = await _globusBankClientIntegration.NameEnquiryAsync(receipient.AccountNumber, receipient.RoutingNumber);
        if (nameEnquiry is null || string.IsNullOrEmpty(nameEnquiry.responsecode))
        {
            return new ApiResponse
            {
                Message = "Unable to validate account number"
            };
        }

        if (nameEnquiry.responsecode != "00")
        {
            return new ApiResponse
            {
                Message = "Unable to validate account number"
            };
        }

        var doRequest = new GlobusSingleTransferRequest
        {
            Amount = (double)transfer.Amount,
            SourceAccount = StaticData.GlobusLinkedAccount,
            SourceAccountName = receipient.AccountName,
            SourceCcy = transfer.Currency,
            DestinationAccount = receipient.AccountNumber,
            DestinationAccountName = receipient.AccountName,
            DestinationCcy = transfer.Currency,
            DestinationBankCode = receipient.RoutingNumber,
            TransId = transfer.Reference,
            AppUser = "tparty",
            Narration = account.Name,
            NipNameEnqRef = nameEnquiry.result.sessionid
        };

        var response = await _globusBankClientIntegration.SingleTransferAsync(doRequest);
        if (response is null || string.IsNullOrEmpty(response.responsecode))
        {
            return new ApiResponse
            {
                Message = "Unable to access transfer service"
            };
        }

        if (response.responsemessage != TransactionStatuses.Success)
        {
            return new ApiResponse { Message = response.responsemessage, Data = response };
        }

        return new ApiResponse { Message = "Transfer processed successfully", Success = true, Data = response.result };
    }

    public async Task<ApiResponse> CreateVirtualAccFullAsync(GlobusVirtualAccountFullRequest request)
    {
        var virtualAccount = await _globusBankClientIntegration.CreateVirtualAccountFullAsync(request);
        if (virtualAccount is null)
        {
            return new ApiResponse
            {
                Message = "Unable to create account"
            };
        }

        return new ApiResponse { Data = virtualAccount.Result, Message = "Successful", Success = true };
    }

    public async Task<ApiResponse> CreateVirtualAccLiteAsync(GlobusVirtualAccountLiteRequest request)
    {
        var virtualAccount = await _globusBankClientIntegration.CreateVirtualAccountLiteAsync(request);
        if (virtualAccount is null)
        {
            return new ApiResponse
            {
                Message = "Unable to create account"
            };
        }

        return new ApiResponse { Data = virtualAccount.result, Message = "Successful", Success = true };
    }

    public async Task<ApiResponse> CreateVirtualAccMaxAsync(GlobusVirtualAccountMaxRequest request)
    {
        var virtualAccount = await _globusBankClientIntegration.CreateVirtualAccountMaxAsync(request);
        if (virtualAccount is null)
        {
            return new ApiResponse
            {
                Message = "Unable to generate account"
            };
        }

        return new ApiResponse { Data = virtualAccount.result, Message = "Successful", Success = true };
    }

    public async Task TransferAsync(DoTransferRequest request)
    {
        var transfer = request.Transfer;

        var globusRequest = new GlobusSingleTransferRequest()
        {

        };

        var response = await _globusBankClientIntegration.SingleTransferAsync(globusRequest);
        if (response.responsecode != "00")
        {

        }
    }

    public async Task<ApiResponse> GenerateAsync(Account account)
    {
        //Generate UUID
        string reference = Guid.NewGuid().ToString();

        var response = await CreateVirtualAccMaxAsync(new GlobusVirtualAccountMaxRequest
        {
            AccountName = account.Name,
            PartnerReference = reference,
            LinkedPartnerAccountNumber = StaticData.GlobusLinkedAccount
        });

        if (!response.Success) return response;

        var virtualAccount = new VirtualAccount
        {
            AccountId = account.Id,
            AccountHolderName = account.Name,
            AccountNumber = response.Data.ToString(),
            BankName = "Globus",
            Country = Countries.Nigeria,
            CreatedAt = DateTime.Now,
            Currency = Currencies.NGN,
            RoutingNumber = "103",
            Status = AccountStatuses.Active,
            Type = "Local",
            ReferenceCode = reference
        };

        bool insert = await _virtualAccountRepository.InsertAsync(virtualAccount);
        if (!insert)
        {
            return new ApiResponse
            {
                Message = "Unable to save account details"
            };
        }

        return new ApiResponse { Success = true, Message = "Account generated successfully" };
    }
}
