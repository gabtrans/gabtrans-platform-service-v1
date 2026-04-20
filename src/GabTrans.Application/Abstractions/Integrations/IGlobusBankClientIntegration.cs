using System;
using GabTrans.Application.DataTransfer.GlobusBank;

namespace GabTrans.Application.Interfaces.Integrations;

public interface IGlobusBankClientIntegration
{
    Task<bool> GenerateTokenAsync();
    Task<GlobusStateListResponse> GetAllStateAsync();
    Task<GlobusCitiesResponse> GetCitiesAsync(long stateid);
    Task<GlobusAccountBalanceResponse> AccountBalanceAsync(string accountNo);
    Task<GlobusNameEnquiryResponse> NameEnquiryAsync(string accountno, string bankcode);
    Task<GlobusBankListResponse> BanksAsync();
    Task<GlobusTransactionQueryServiceResponse> TransactionQueryAsync(string transid);
    Task<GlobusVirtualAccountResponse> CreateVirtualAccountFullAsync(GlobusVirtualAccountFullRequest request);
    Task<GlobusVirtualLiteResponse> CreateVirtualAccountLiteAsync(GlobusVirtualAccountLiteRequest request);
    Task<GlobusVirtualAccountMaxResponse> CreateVirtualAccountMaxAsync(GlobusVirtualAccountMaxRequest request);
    Task<GlobusSingleTransferResponse> SingleTransferAsync(GlobusSingleTransferRequest request);
}
