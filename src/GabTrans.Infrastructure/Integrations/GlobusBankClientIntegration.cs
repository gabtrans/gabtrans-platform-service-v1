using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer.GlobusBank;
using GabTrans.Application.Interfaces.Integrations;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;

namespace GabTrans.Infrastructure.Integrations;

public class GlobusBankClientIntegration : IGlobusBankClientIntegration
{
    private string BasicAuth;
    private readonly HttpClient _client;
    private readonly ILogService _logService;
    private readonly IEncryptionService _encryptionService;

    public GlobusBankClientIntegration(HttpClient httpClient, ILogService logService, IEncryptionService encryptionService)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _client.DefaultRequestHeaders.Add("User-Agent", "Custom User Agent");
        _client.Timeout = TimeSpan.FromMinutes(30);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{StaticData.GlobusBankClientId}{StaticData.GlobusBankSecretKey}"));
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    public async Task<T> GetAsync<T>(string relativePath)
    {
        Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
        var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Get };

        request.Headers.Add("clientid", $"{StaticData.GlobusBankClientId}");

        request.Headers.Add("Authorization", $"Bearer {StaticData.GlobusSessionToken.Token}");

        // Console.WriteLine("Request:: " + JsonConvert.SerializeObject( content));

        // if (content != null) request.Content = new FormUrlEncodedContent(content);

        var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        var data = await response.Content.ReadAsStringAsync();

        _logService.LogInfo("GlobusBankClientIntegration", "GetAsync", "Response from Globus Bank:: " + data);

        return JsonConvert.DeserializeObject<T>(data);
    }

    private static Uri CreateRequestUri(string relativePath, string queryString = "")
    {
        if (!relativePath.Contains("token"))
        {
            var endpoint = new Uri(string.Concat(StaticData.GlobusBankBaseURL, relativePath));
            var uriBuilder = new UriBuilder(endpoint);
            if (!string.IsNullOrEmpty(queryString))
            {
                uriBuilder.Query = queryString;
            }
            return uriBuilder.Uri;
        }

        var baseUrl = new Uri(relativePath);
        var baseUriBuilder = new UriBuilder(baseUrl);
        if (!string.IsNullOrEmpty(queryString))
        {
            baseUriBuilder.Query = queryString;
        }
        return baseUriBuilder.Uri;
    }

    /// <summary>
    /// Common method for making POST calls
    /// </summary>
    public async Task<T> PostAsync<T>(string relativePath, Dictionary<string, string> content)
    {
        Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));

        var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post, Content = new FormUrlEncodedContent(content) };

        request.Headers.Add("ClientId", $"{StaticData.GlobusBankClientId}");

        if (StaticData.GlobusSessionToken != null && !string.IsNullOrEmpty(StaticData.GlobusSessionToken.Token))
        {
            request.Headers.Add("Authorization", $"Bearer {StaticData.GlobusSessionToken.Token}");
        }

        _logService.LogInfo(nameof(GlobusBankClientIntegration), nameof(PostAsync), JsonConvert.SerializeObject(request));
        // if (encryptedPayload is not null) request.Headers.Add("Encryption", encryptedPayload);

        var baseResponse = await _client.SendAsync(request);
        var responseData = await baseResponse.Content.ReadAsStringAsync();

        _logService.LogInfo("GlobusBankClientIntegration", "PostAsync", "Response from Globus Bank:: " + responseData);

        return JsonConvert.DeserializeObject<T>(responseData);
    }

    public async Task<T> PostAsync<T>(string relativePath, object content)
    {

        Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));

        var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post, Content = CreateHttpContent(content) };

        // request.Headers.Add("Accept", "*/*");
        request.Headers.Add("ClientId", $"{StaticData.GlobusBankClientId}");

        request.Headers.Add("Authorization", $"Bearer {StaticData.GlobusSessionToken.Token}");

        var baseResponse = await _client.SendAsync(request);

        var responseData = await baseResponse.Content.ReadAsStringAsync();

        _logService.LogInfo("GlobusBankClientIntegration", "PostAsync", "Response from Globus Bank:: " + responseData);

        return JsonConvert.DeserializeObject<T>(responseData);
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

    public async Task<bool> GenerateTokenAsync()
    {
        try
        {
            var username = $"{DateTime.UtcNow.Date.ToString("yyyyMMdd")}{StaticData.GlobusBankClientId}";
            var password = StaticData.GlobusBankClientId;

            var hashedUsername = _encryptionService.ToSha256(username);
            var hashedPassword = _encryptionService.ToSha256(password);

            if (StaticData.GlobusSessionToken is not null && !string.IsNullOrEmpty(StaticData.GlobusSessionToken.Token))
            {
                int timeDifference = DateDiff(DateInterval.Minute, StaticData.GlobusSessionToken.CreatedAt, DateTime.Now);
                if (timeDifference <= 50)
                {
                    Console.WriteLine($"Session token Globus Bank is active and have been used for " + timeDifference);
                    return true;
                }
            }

            var request = new GlobusBankAuthRequest
            {
                grant_type = StaticData.GlobusBankGrantType,
                username = hashedUsername,
                password = hashedPassword,
                client_id = StaticData.GlobusBankClientId,
                client_secret = StaticData.GlobusBankSecretKey,
                scope = StaticData.GlobusBankScope
            };

            //Console.WriteLine("ClientId for Globus Bank:: " + StaticData.GlobusBankClientId);
            //Console.WriteLine("ClientSecret :: " + StaticData.GlobusBankSecretKey);

            var serializedModel = JsonConvert.SerializeObject(request);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedModel);

            _logService.LogInfo("GlobusBankClientIntegration", "GenerateTokenAsync", "Auth request to Globus Bank:: " + serializedModel);

            var response = await PostAsync<GlobusBankAuthenticationResponse>(StaticData.GlobusBankTokenEndpoint, payload);
            if (response == null || string.IsNullOrEmpty(response.access_token)) return false;

            StaticData.GlobusSessionToken = new GlobusSessionToken
            {
                Token = response.access_token,
                CreatedAt = DateTime.Now,
                Expires = response.expires_in
            };

            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "GenerateTokenAsync", ex);
        }

        return false;
    }

    public int DateDiff(DateInterval interval, DateTime? date, DateTime date2)
    {
        TimeSpan ts = ts = date2 - date.GetValueOrDefault();

        return interval switch
        {
            DateInterval.Year => date2.Year - date.Value.Year,
            DateInterval.Month => (date2.Month - date.Value.Month) + (12 * (date2.Year - date.Value.Year)),
            DateInterval.Weekday => (int)Fix(ts.TotalDays) / 7,
            DateInterval.Day => (int)Fix(ts.TotalDays),
            DateInterval.Hour => (int)Fix(ts.TotalHours),
            DateInterval.Minute => (int)Fix(ts.TotalMinutes),
            _ => (int)Fix(ts.TotalSeconds),
        };
    }

    private static long Fix(double Number)
    {
        if (Number >= 0)
        {
            return (long)Math.Floor(Number);
        }
        return (long)Math.Ceiling(Number);
    }

    public async Task<GlobusStateListResponse> GetAllStateAsync()
    {
        try
        {
            await GenerateTokenAsync();
            var response = await GetAsync<GlobusStateListResponse>($"/api/v2/get-states ");
            if (response is null || response.data is null) return new GlobusStateListResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "GetAllStateAsync", ex);
        }
        return new GlobusStateListResponse();
    }

    public async Task<GlobusCitiesResponse> GetCitiesAsync(long stateId)
    {
        try
        {
            await GenerateTokenAsync();
            var response = await GetAsync<GlobusCitiesResponse>($"/api/v2/city/{stateId}");
            if (response is null || response.data is null) return new GlobusCitiesResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "GetCitiesAsync", ex);
        }
        return new GlobusCitiesResponse();
    }

    public async Task<GlobusAccountBalanceResponse> AccountBalanceAsync(string accountNo)
    {
        try
        {
            _logService.LogInfo("GlobusBankClientIntegration", "AccountBalanceAsync", "Account balance");

            bool authenticated = await GenerateTokenAsync();
            if (!authenticated) return new GlobusAccountBalanceResponse();

            var response = await GetAsync<GlobusAccountBalanceResponse>($"/api/v2/account-balance/{accountNo}");
            if (response is null || string.IsNullOrEmpty(response.responsecode)) return new GlobusAccountBalanceResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "AccountBalanceAsync", ex);
        }
        return new GlobusAccountBalanceResponse();
    }

    public async Task<GlobusNameEnquiryResponse> NameEnquiryAsync(string accountNo, string bankCode)
    {
        try
        {
            _logService.LogInfo("GlobusBankClientIntegration", "NameEnquiryAsync", "Name enquiry");

            await GenerateTokenAsync();
            var response = await GetAsync<GlobusNameEnquiryResponse>($"/api/v2/name-enquiry/{accountNo}/{bankCode}");
            if (response is null || string.IsNullOrEmpty(response.responsecode)) return new GlobusNameEnquiryResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "NameEnquiryAsync", ex);
        }
        return new GlobusNameEnquiryResponse();
    }

    public async Task<GlobusBankListResponse> BanksAsync()
    {
        try
        {
            await GenerateTokenAsync();
            var response = await GetAsync<GlobusBankListResponse>($"/api/v2/banks");
            if (response is null || string.IsNullOrEmpty(response.responsecode)) return new GlobusBankListResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "BanksAsync", ex);
        }
        return new GlobusBankListResponse();
    }

    public async Task<GlobusTransactionQueryServiceResponse> TransactionQueryAsync(string transId)
    {
        try
        {
            bool authenticated = await GenerateTokenAsync();
            if (!authenticated) return new GlobusTransactionQueryServiceResponse();

            var response = await GetAsync<GlobusTransactionQueryServiceResponse>($"/api/v2/transaction-status/tparty/{transId}");
            if (response is null || string.IsNullOrEmpty(response.statusCode)) return new GlobusTransactionQueryServiceResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "GlobusVirtualAccountResponse", ex);
        }
        return new GlobusTransactionQueryServiceResponse();
    }

    public async Task<GlobusVirtualAccountResponse> CreateVirtualAccountFullAsync(GlobusVirtualAccountFullRequest request)
    {
        try
        {
            await GenerateTokenAsync();

            var serializedModel = JsonConvert.SerializeObject(request);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedModel);

            var response = await PostAsync<GlobusVirtualAccountResponse>($"/api/v2/virtual-account", payload);
            if (response is null || string.IsNullOrEmpty(response.Responsecode)) return new GlobusVirtualAccountResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "CreateVirtualAccountFullAsync", ex);
        }
        return new GlobusVirtualAccountResponse();
    }

    public async Task<GlobusVirtualLiteResponse> CreateVirtualAccountLiteAsync(GlobusVirtualAccountLiteRequest request)
    {
        try
        {
            await GenerateTokenAsync();
            var response = await PostAsync<GlobusVirtualLiteResponse>($"/api/v2/virtual-account-lite", request);
            if (response is null || string.IsNullOrEmpty(response.responsecode)) return new GlobusVirtualLiteResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "CreateVirtualAccountLiteAsync", ex);
        }
        return new GlobusVirtualLiteResponse();
    }

    public async Task<GlobusVirtualAccountMaxResponse> CreateVirtualAccountMaxAsync(GlobusVirtualAccountMaxRequest request)
    {
        try
        {
            await GenerateTokenAsync();
            var response = await PostAsync<GlobusVirtualAccountMaxResponse>($"/api/v2/virtual-account-max", request);
            if (response is null || response.result is null) return new GlobusVirtualAccountMaxResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "CreateVirtualAccountLiteAsync", ex);
        }
        return new GlobusVirtualAccountMaxResponse();
    }

    public async Task<GlobusSingleTransferResponse> SingleTransferAsync(GlobusSingleTransferRequest request)
    {
        try
        {
            _logService.LogInfo("GlobusBankClientIntegration", "SingleTransferAsync", $"Transfer:: Payload:: {JsonConvert.SerializeObject(request)}");

            bool authenticated = await GenerateTokenAsync();
            if (!authenticated) return new GlobusSingleTransferResponse();

            var serializedModel = JsonConvert.SerializeObject(request);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedModel);
            var response = await PostAsync<GlobusSingleTransferResponse>($"/api/v2/single-transfer", request);
            if (response is null || string.IsNullOrEmpty(response.responsecode)) return new GlobusSingleTransferResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logService.LogError("GlobusBankClientIntegration", "SingleTransferAsync", ex);
        }
        return new GlobusSingleTransferResponse();
    }
}
