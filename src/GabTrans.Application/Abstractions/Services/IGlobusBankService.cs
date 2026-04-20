using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.GlobusBank;
using GabTrans.Domain.Entities;
using System;

namespace GabTrans.Application.Interfaces.Services;

public interface IGlobusBankService
{
    Task<ApiResponse> BankListAsync();
    Task<ApiResponse> GenerateAsync(Account account);
    Task<ApiResponse> GetCitiesAsync(long stateid);
    Task<ApiResponse> GetStatesAsync();
    Task<GlobusAccountBalanceResponse> GetBalanceAsync(string accountNo);
    Task<ApiResponse> GetNameEnquiryAsync(string accountno, string bankcode);
    Task<ApiResponse> TransactionQueryAsync(Transfer transfer);
    Task<GlobusSingleTransferResponse> SingleTransferAsync(GlobusSingleTransferRequest request);
    Task<ApiResponse> DoTransferAsync(DoTransferRequest request);
    Task<ApiResponse> CreateVirtualAccFullAsync(GlobusVirtualAccountFullRequest request);
    Task<ApiResponse> CreateVirtualAccLiteAsync(GlobusVirtualAccountLiteRequest request);
    Task<ApiResponse> CreateVirtualAccMaxAsync(GlobusVirtualAccountMaxRequest request);
    // Task TestAsync();
}
