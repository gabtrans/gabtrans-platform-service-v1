using GabTrans.Application.DataTransfer.Infinitus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IInfinitusClientIntegration
    {
        Task<GetClientInfinitusDetailsResponse> GetClientAsync(string clientId);
        Task<List<DocumentInfinitusResponse>> DocumentClientAsync(string clientId, Stream fileStream, string fileName, string type);
        Task<List<DocumentInfinitusResponse>> DocumentBusinessClientAsync(string clientId, Stream fileStream, string fileName, string type);
        Task<CreateClientInfinitusResponse> CreateClientAsync(CreateInfinitusClientRequest request);
        Task<UpdatePersonalInfinitusClientResponse> UpdatePersonalClientAsync(UpdatePersonalInfinitusClientRequest request, string clientId);
        Task<UpdateBusinessInfinitusClientResponse> UpdateBusinessClientAsync(UpdateBusinessInfinitusClientRequest request, string clientId);
        Task<SubmitInfinitusClientResponse> SubmitClientAsync(SubmitInfinitusClientRequest request, string clientId);
        Task<RepresentativeInfinitusResponse> GetRepresentativeAsync(string clientId, string representativeId);
        Task<RepresentativeInfinitusResponse> DeleteRepresentativeAsync(string clientId, string representativeId);
        Task<List<DocumentInfinitusResponse>> DocumentRepresentativeAsync(string clientId, string representativeId, Stream fileStream, string fileName, string type);
        Task<RepresentativeInfinitusResponse> CreateRepresentativeAsync(RepresentativeInfinitusRequest request, string clientId);
        Task<RepresentativeInfinitusResponse> UpdateRepresentativeAsync(RepresentativeInfinitusRequest request, string clientId, string representativeId);
             Task<List<InfinitusWalletBalanceResponse>> GetWalletBalanceAsync(string accountId);
        Task<ConversionInfinitusResponse> CreateConversionAsync(CreateConversionInfinitusRequest request, string accountId);
        Task<ConversionInfinitusResponse> GetConversionAsync(string conversionId, string accountId);
        Task<InfinitusTradeResponse> TradeAsync(InfinitusTradeRequest request, string accountId);
        Task<InfinitusCryptoAccountResponse> CryptoAccountAsync(InfinitusCryptoAccountRequest request, string accountId);
        Task<InfinitusDepositResponse> DepositAsync(string depositId, string accountId);
        Task<GetInfinitusTransactionResponse> GetTransactionsAsync(GetInfinitusTransactionRequest request, string accountId);
        Task<InfinitoGlobalAccountResponse> CreateGlobalAccountAsync(CreateInfinitoGlobalAccountRequest request, string accountId);
        Task<InfinitoGlobalAccountResponse> GlobalAccountAsync(string globalAccountId, string accountId);
        Task<GetInfinitusRecipientSchemaResponse> GetRecipientSchemaAsync(GetInfinitusRecipientSchemaRequest request, string accountId);
        Task<InfinitusRecipientResponse> CreateRecipientAsync(CreateInfinitusRecipientRequest request, string accountId);
        Task<InfinitusRecipientResponse> UpdateRecipientAsync(CreateInfinitusRecipientRequest request,string recipientId, string accountId);
        Task<InfinitusRecipientResponse> DeleteRecipientAsync(string recipientId, string accountId);
        Task<InfinitusRecipientResponse> GetRecipientAsync(string recipientId, string accountId);
        Task<InfinitusPayoutResponse> PayoutAsync(CreateInfinitusPayoutRequest request, string accountId);
        Task<InfinitusPayoutResponse> GetPayoutAsync(string payoutId, string accountId);
        Task<InfinitusAccountResponse> GetAccountAsync(string accountId);
        Task<InfinitusAccountRequestResponse> GetAccountRequestAsync(string requestId);
        Task<InfinitusAccountRequestsResponse> GetAccountRequestsAsync(string clientId);
        Task<CreateInfinitusWebhookResponse> CreateWebhookAsync(CreateInfinitusWebhookRequest request);
        Task<SimulateInfinitusTopupResponse> SimulateTopupAsync(SimulateInfinitusTopupRequest request,string accountId);
        //Task<List<DocumentInfinitusResponse>> PersonalDocumentClientAsync(string clientId, string type, string filePath);
    }
}
