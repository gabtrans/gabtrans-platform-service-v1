using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IInfinitusService
    {
        Task TestAsync();
        Task<ApiResponse> GetClientAsync(long userId);
        Task<ApiResponse> DocumentClientAsync(Kyc kyc);
        Task<ApiResponse> CreateClientAsync(User user, string type);
        Task<ApiResponse> UpdatePersonalClientAsync(User user);
        Task<ApiResponse> UpdateBusinessClientAsync(Business business, long userId);
        Task<ApiResponse> SubmitClientAsync(Kyc kyc, List<string> providers);
        Task<ApiResponse> GetRepresentativeAsync(string clientId, string representativeId);
        Task<ApiResponse> DeleteRepresentativeAsync(string clientId, string representativeId);
        Task<ApiResponse> DocumentRepresentativeAsync(Kyc kyc);
        Task<ApiResponse> DocumentBusinessClientAsync(Kyc kyc);
        Task<ApiResponse> CreateRepresentativeAsync(User user);
        Task<ApiResponse> UpdateRepresentativeAsync(User user);
        Task<ApiResponse> GetWalletBalanceAsync(long accountId);
        Task<ApiResponse> CreateConversionAsync(TradeFxRequest request, string accountId);
        Task<ApiResponse> GetConversionAsync(string conversionId, string accountId);
        Task<ApiResponse> TradeAsync(CryptoTrade cryptoTrade, long accountId, string walletId);
        Task<ApiResponse> DepositAsync(string accountUuid, string depositId);
        Task<ApiResponse> CreateGlobalAccountAsync(Account account, string currency, string country);
        Task<ApiResponse> GetGlobalAccountAsync(long accountId, string currency);
        Task<ApiResponse> GetRecipientSchemaAsync(GetTransferSchemaRequest request, long userId);
        Task<ApiResponse> CreateRecipientAsync(TransferRecipient recipient, long accountId);
        Task<ApiResponse> UpdateRecipientAsync(TransferRecipient recipient);
        Task<ApiResponse> DeleteRecipientAsync(string recipientId, string accountId);
        Task<ApiResponse> GetRecipientAsync(long recipientId, long accountId);
        Task<ApiResponse> TransferAsync(Transfer transfer, string accountUuId);
        Task<ApiResponse> CreateTransferAsync(Transfer transfer, string recipientUuid);
        Task<ApiResponse> GetAccountAsync(string accountId);
        Task<ApiResponse> GetAccountRequestAsync(Kyc kyc);
        Task<ApiResponse> GetAccountRequestsAsync(Kyc kyc);
        Task<ApiResponse> GetTransactionsAsync(string accountUuid, string type);
    }
}
