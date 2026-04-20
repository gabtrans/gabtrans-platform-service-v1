using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Newtonsoft.Json;
using NPOI.HPSF;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace GabTrans.Infrastructure.Integrations
{
    public class InfinitusClientIntegration : IInfinitusClientIntegration
    {
        private readonly HttpClient _client;
        private readonly ILogService _logService;
        public InfinitusClientIntegration(HttpClient httpClient, ILogService logService)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _client.DefaultRequestHeaders.Add("User-Agent", "Custom User Agent");
            _client.Timeout = TimeSpan.FromMinutes(30);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<T> GetAsync<T>(string relativePath, string? accountId = null)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Get };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);
            if (!string.IsNullOrEmpty(accountId)) request.Headers.Add("x-account", accountId);

            // if (content != null) request.Content = new FormUrlEncodedContent(content);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(GetAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> GetAsync<T>(string relativePath, object content, string? accountId = null)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Get, Content = CreateHttpContent(content) };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);
            if (!string.IsNullOrEmpty(accountId)) request.Headers.Add("x-account", accountId);

            // if (content != null) request.Content = new FormUrlEncodedContent(content);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(GetAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }

        private static Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(string.Concat(StaticData.InfintusURL, relativePath));
            var uriBuilder = new UriBuilder(endpoint);
            if (!string.IsNullOrEmpty(queryString))
            {
                uriBuilder.Query = queryString;
            }
            return uriBuilder.Uri;
        }

        public async Task<T> UploadAsync<T>(string relativePath, Stream fileStream, string fileName, string fileType)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent($"{fileType}"), "type");

            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            content.Add(streamContent, "file", $"{fileName}");
            request.Content = content;

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(UploadAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        public async Task<T> PostAsync<T>(string relativePath, object content)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post, Content = CreateHttpContent(content) };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);
            //if (token is not null) request.Headers.Add("Bearer", token);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(PostAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> PostAsync<T>(string relativePath, object content, string accountId)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post, Content = CreateHttpContent(content) };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);

            request.Headers.Add("x-account", accountId);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(PostAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Common method for making PUT calls
        /// </summary>
        public async Task<T> PutAsync<T>(string relativePath, object content)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Put, Content = CreateHttpContent(content) };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            _logService.LogInfo(nameof(InfinitusClientIntegration), nameof(PutAsync), data);

            return JsonConvert.DeserializeObject<T>(data);
        }


        /// <summary>
        /// Common method for making DELETE calls
        /// </summary>
        public async Task<T> DeleteAsync<T>(string relativePath)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Delete };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);
            //if (token is not null) request.Headers.Add("Bearer", token);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> DeleteAsync<T>(string relativePath, string accountId)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Delete };

            request.Headers.Add("x-api-key", StaticData.InfintusApiKey);

            request.Headers.Add("x-account", accountId);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }


        private static HttpContent CreateHttpContent(object content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public async Task<CreateClientInfinitusResponse> CreateClientAsync(CreateInfinitusClientRequest request)
        {
            try
            {
                var response = await PostAsync<CreateClientInfinitusResponse>("/v1/platform/client", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new CreateClientInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateClientAsync), ex);
            }

            return new CreateClientInfinitusResponse();
        }

        public async Task<GetClientInfinitusDetailsResponse> GetClientAsync(string clientId)
        {
            try
            {
                var response = await GetAsync<GetClientInfinitusDetailsResponse>($"/v1/platform/client/{clientId}");
                if (response is null || string.IsNullOrEmpty(response.id)) return new GetClientInfinitusDetailsResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetClientAsync), ex);
            }

            return new GetClientInfinitusDetailsResponse();
        }

        public async Task<UpdatePersonalInfinitusClientResponse> UpdatePersonalClientAsync(UpdatePersonalInfinitusClientRequest request, string clientId)
        {
            try
            {
                var response = await PutAsync<UpdatePersonalInfinitusClientResponse>($"/v1/platform/client/personal/{clientId}", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new UpdatePersonalInfinitusClientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(UpdatePersonalClientAsync), ex);
            }

            return new UpdatePersonalInfinitusClientResponse();
        }

        public async Task<UpdateBusinessInfinitusClientResponse> UpdateBusinessClientAsync(UpdateBusinessInfinitusClientRequest request, string clientId)
        {
            try
            {
                var response = await PutAsync<UpdateBusinessInfinitusClientResponse>($"/v1/platform/client/business/{clientId}", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new UpdateBusinessInfinitusClientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(UpdateBusinessClientAsync), ex);
            }

            return new UpdateBusinessInfinitusClientResponse();
        }

        public async Task<SubmitInfinitusClientResponse> SubmitClientAsync(SubmitInfinitusClientRequest request, string clientId)
        {
            try
            {
                var response = await PostAsync<SubmitInfinitusClientResponse>($"/v1/platform/client/{clientId}/submit", request);
                if (response is null || string.IsNullOrEmpty(response.Message)) return new SubmitInfinitusClientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(SubmitClientAsync), ex);
            }

            return new SubmitInfinitusClientResponse();
        }

        public async Task<List<DocumentInfinitusResponse>> PersonalDocumentClientAsync(string clientId,Stream fileStream,string fileName, string type)
        {
            try
            {
                var response = await UploadAsync<List<DocumentInfinitusResponse>>($"/v1/platform/client/personal/{clientId}/document", fileStream,fileName, type);
                if (response is null || response.Count == 0) return new List<DocumentInfinitusResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(PersonalDocumentClientAsync), ex);
            }

            return new List<DocumentInfinitusResponse>();
        }

        public async Task<List<DocumentInfinitusResponse>> DocumentClientAsync(string clientId, Stream fileStream, string fileName, string type)
        {
            try
            {
                var response = await UploadAsync<List<DocumentInfinitusResponse>>($"/v1/platform/client/personal/{clientId}/document", fileStream, fileName, type);
                if (response is null || response.Count == 0) return new List<DocumentInfinitusResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DocumentClientAsync), ex);
            }

            return new List<DocumentInfinitusResponse>();
        }

        public async Task<List<DocumentInfinitusResponse>> DocumentBusinessClientAsync(string clientId, Stream fileStream, string fileName, string type)
        {
            try
            {
                var response = await UploadAsync<List<DocumentInfinitusResponse>>($"/v1/platform/client/business/{clientId}/document", fileStream, fileName, type);
                if (response is null || response.Count == 0) return new List<DocumentInfinitusResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DocumentClientAsync), ex);
            }

            return new List<DocumentInfinitusResponse>();
        }

        public async Task<RepresentativeInfinitusResponse> GetRepresentativeAsync(string clientId, string representativeId)
        {
            try
            {
                var response = await GetAsync<RepresentativeInfinitusResponse>($"/v1/platform/client/business/{clientId}/representative/{representativeId}");
                if (response is null || string.IsNullOrEmpty(response.id)) return new RepresentativeInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetRepresentativeAsync), ex);
            }

            return new RepresentativeInfinitusResponse();
        }

        public async Task<RepresentativeInfinitusResponse> DeleteRepresentativeAsync(string clientId, string representativeId)
        {
            try
            {
                var response = await DeleteAsync<RepresentativeInfinitusResponse>($"/v1/platform/client/business/{clientId}/representative/{representativeId}");
                if (response is null || string.IsNullOrEmpty(response.id)) return new RepresentativeInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DeleteRepresentativeAsync), ex);
            }

            return new RepresentativeInfinitusResponse();
        }

        public async Task<List<DocumentInfinitusResponse>> DocumentRepresentativeAsync(string clientId, string representativeId, Stream fileStream, string fileName, string type)
        {
            try
            {
                var response = await UploadAsync<List<DocumentInfinitusResponse>>($"/v1/platform/client/business/{clientId}/representative/{representativeId}/document", fileStream, fileName, type);
                if (response is null || response.Count == 0) return new List<DocumentInfinitusResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DocumentRepresentativeAsync), ex);
            }

            return new List<DocumentInfinitusResponse>();
        }

        public async Task<RepresentativeInfinitusResponse> CreateRepresentativeAsync(RepresentativeInfinitusRequest request, string clientId)
        {
            try
            {
                var response = await PostAsync<RepresentativeInfinitusResponse>($"/v1/platform/client/business/{clientId}/representative", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new RepresentativeInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateRepresentativeAsync), ex);
            }

            return new RepresentativeInfinitusResponse();
        }

        public async Task<RepresentativeInfinitusResponse> UpdateRepresentativeAsync(RepresentativeInfinitusRequest request, string clientId, string id)
        {
            try
            {
                var response = await PostAsync<RepresentativeInfinitusResponse>($"/v1/platform/client/business/{clientId}/representative/{id}", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new RepresentativeInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(UpdateRepresentativeAsync), ex);
            }

            return new RepresentativeInfinitusResponse();
        }

        public async Task<List<InfinitusWalletBalanceResponse>> GetWalletBalanceAsync(string accountId)
        {
            try
            {
                var response = await GetAsync<List<InfinitusWalletBalanceResponse>>($"/v1/banking/wallet", accountId);
                if (response is null || response.Count == 0) return new List<InfinitusWalletBalanceResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetWalletBalanceAsync), ex);
            }

            return new List<InfinitusWalletBalanceResponse>();
        }

        public async Task<ConversionInfinitusResponse> CreateConversionAsync(CreateConversionInfinitusRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<ConversionInfinitusResponse>("/v1/banking/conversion", request, accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new ConversionInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateConversionAsync), ex);
            }

            return new ConversionInfinitusResponse();
        }

        public async Task<ConversionInfinitusResponse> GetConversionAsync(string conversionId, string accountId)
        {
            try
            {
                var response = await GetAsync<ConversionInfinitusResponse>($"/v1/banking/conversion/{conversionId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new ConversionInfinitusResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetConversionAsync), ex);
            }

            return new ConversionInfinitusResponse();
        }

        public async Task<InfinitusTradeResponse> TradeAsync(InfinitusTradeRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<InfinitusTradeResponse>($"/v1/banking/crypto/trade", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusTradeResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(TradeAsync), ex);
            }

            return new InfinitusTradeResponse();
        }

        public async Task<InfinitusDepositResponse> DepositAsync(string depositId, string accountId)
        {
            try
            {
                var response = await GetAsync<InfinitusDepositResponse>($"/v1/banking/deposit/{depositId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusDepositResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DepositAsync), ex);
            }

            return new InfinitusDepositResponse();
        }

        public async Task<InfinitoGlobalAccountResponse> CreateGlobalAccountAsync(CreateInfinitoGlobalAccountRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<InfinitoGlobalAccountResponse>($"/v1/banking/global-account",request, accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitoGlobalAccountResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GlobalAccountAsync), ex);
            }

            return new InfinitoGlobalAccountResponse();
        }

        public async Task<InfinitoGlobalAccountResponse> GlobalAccountAsync(string globalAccountId, string accountId)
        {
            try
            {
                var response = await GetAsync<InfinitoGlobalAccountResponse>($"/v1/banking/global-account/{globalAccountId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitoGlobalAccountResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GlobalAccountAsync), ex);
            }

            return new InfinitoGlobalAccountResponse();
        }

        public async Task<InfinitusCryptoAccountResponse> CryptoAccountAsync(InfinitusCryptoAccountRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<InfinitusCryptoAccountResponse>($"/v1/banking/global-account/{request.id}/crypto", request, accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusCryptoAccountResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CryptoAccountAsync), ex);
            }

            return new InfinitusCryptoAccountResponse();
        }

        public async Task<GetInfinitusRecipientSchemaResponse> GetRecipientSchemaAsync(GetInfinitusRecipientSchemaRequest request, string accountId)
        {
            try
            {
                var response = await GetAsync<GetInfinitusRecipientSchemaResponse>("/v1/banking/recipient/schema", accountId);
                if (response is null || response.fields.Count == 0) return new GetInfinitusRecipientSchemaResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetRecipientSchemaAsync), ex);
            }

            return new GetInfinitusRecipientSchemaResponse();
        }

        public async Task<InfinitusRecipientResponse> CreateRecipientAsync(CreateInfinitusRecipientRequest request, string accountId)
        {
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(request));
                var response = await PostAsync<InfinitusRecipientResponse>("/v1/banking/recipient", request, accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusRecipientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateRecipientAsync), ex);
            }

            return new InfinitusRecipientResponse();
        }

        public async Task<InfinitusRecipientResponse> UpdateRecipientAsync(CreateInfinitusRecipientRequest request, string recipientId, string accountId)
        {
            try
            {
                var response = await PostAsync<InfinitusRecipientResponse>($"/v1/banking/recipient/{recipientId}", request, accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusRecipientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(UpdateRecipientAsync), ex);
            }

            return new InfinitusRecipientResponse();
        }

        public async Task<InfinitusRecipientResponse> DeleteRecipientAsync(string recipientId, string accountId)
        {
            try
            {
                var response = await DeleteAsync<InfinitusRecipientResponse>($"/v1/banking/recipient/{recipientId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusRecipientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(DeleteRecipientAsync), ex);
            }

            return new InfinitusRecipientResponse();
        }

        public async Task<InfinitusRecipientResponse> GetRecipientAsync(string recipientId, string accountId)
        {
            try
            {
                var response = await GetAsync<InfinitusRecipientResponse>($"/v1/banking/recipient/{recipientId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusRecipientResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetRecipientAsync), ex);
            }

            return new InfinitusRecipientResponse();
        }

        public async Task<InfinitusPayoutResponse> PayoutAsync(CreateInfinitusPayoutRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<InfinitusPayoutResponse>("/v1/banking/payout", request, accountId);
                if (response is null || string.IsNullOrEmpty(response.paymentId)) return new InfinitusPayoutResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateRecipientAsync), ex);
            }

            return new InfinitusPayoutResponse();
        }

        public async Task<InfinitusPayoutResponse> GetPayoutAsync(string payoutId, string accountId)
        {
            try
            {
                var response = await GetAsync<InfinitusPayoutResponse>($"/v1/banking/payout/{payoutId}", accountId);
                if (response is null || string.IsNullOrEmpty(response.paymentId)) return new InfinitusPayoutResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetPayoutAsync), ex);
            }

            return new InfinitusPayoutResponse();
        }

        public async Task<InfinitusAccountResponse> GetAccountAsync(string accountId)
        {
            try
            {
                var response = await GetAsync<InfinitusAccountResponse>($"/v1/platform/account/{accountId}");
                if (response is null || string.IsNullOrEmpty(response.name)) return new InfinitusAccountResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(PayoutAsync), ex);
            }

            return new InfinitusAccountResponse();
        }

        public async Task<InfinitusAccountRequestResponse> GetAccountRequestAsync(string requestId)
        {
            try
            {
                var response = await GetAsync<InfinitusAccountRequestResponse>($"/v1/platform/account-request/{requestId}");
                if (response is null || string.IsNullOrEmpty(response.id)) return new InfinitusAccountRequestResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetAccountRequestAsync), ex);
            }

            return new InfinitusAccountRequestResponse();
        }

        public async Task<InfinitusAccountRequestsResponse> GetAccountRequestsAsync(string clientId)
        {
            try
            {
                var response = await GetAsync<InfinitusAccountRequestsResponse>($"/v1/platform/account-request?clientId={clientId}");
                if (response is null || response.items.Count == 0) return new InfinitusAccountRequestsResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetAccountRequestsAsync), ex);
            }

            return new InfinitusAccountRequestsResponse();
        }

        public async Task<GetInfinitusTransactionResponse> GetTransactionsAsync(GetInfinitusTransactionRequest request, string accountId)
        {
            try
            {
                var response = await GetAsync<GetInfinitusTransactionResponse>("/v1/banking/transaction", request);
                if (response is null || response.items.Count == 0) return new GetInfinitusTransactionResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(GetTransactionsAsync), ex);
            }

            return new GetInfinitusTransactionResponse();
        }

        public async Task<CreateInfinitusWebhookResponse> CreateWebhookAsync(CreateInfinitusWebhookRequest request)
        {
            try
            {
                var response = await PostAsync<CreateInfinitusWebhookResponse>("/v1/platform/webhook", request);
                if (response is null || string.IsNullOrEmpty(response.id)) return new CreateInfinitusWebhookResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(CreateWebhookAsync), ex);
            }

            return new CreateInfinitusWebhookResponse();
        }

        public async Task<SimulateInfinitusTopupResponse> SimulateTopupAsync(SimulateInfinitusTopupRequest request, string accountId)
        {
            try
            {
                var response = await PostAsync<SimulateInfinitusTopupResponse>("/v1/banking/simulation/top-up-balance", request,accountId);
                if (response is null || string.IsNullOrEmpty(response.id)) return new SimulateInfinitusTopupResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InfinitusClientIntegration), nameof(SimulateTopupAsync), ex);
            }

            return new SimulateInfinitusTopupResponse();
        }
    }
}
