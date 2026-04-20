using GabTrans.Application.Abstractions.Integrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using HeyRed.Mime;

namespace GabTrans.Infrastructure.Integrations
{
    public class FileStackClientIntegration : IFileStackClientIntegration
    {
        private readonly HttpClient _client;

        public FileStackClientIntegration(HttpClient httpClient)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _client.DefaultRequestHeaders.Add("User-Agent", "Custom User Agent");
            _client.Timeout = TimeSpan.FromMinutes(30);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public async Task<T> GetAsync<T>(string relativePath, Dictionary<string, string>? content = null)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Get };

            // request.Headers.Add("Bearer", _apiCredential.ApiKey);

            if (content != null) request.Content = new FormUrlEncodedContent(content);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }



        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            if (!string.IsNullOrEmpty(queryString))
            {
                uriBuilder.Query = queryString;
            }
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        /// 
        public async Task<T> PostAsync<T>(string relativePath, object content, bool isUrl = false)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage() { RequestUri = requestUrl, Method = HttpMethod.Post };

            if (isUrl) request.Headers.Add("url", (string)content);

            if (!isUrl) request.Headers.Add("Content-Type", "image/png");

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }



        public async Task<T> PostAsync<T>(string relativePath, byte[] fileContent, string contentType)
        {
            Uri requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath));
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            using (var content = new ByteArrayContent(fileContent))
            {
                content.Headers.Add("Content-Type", contentType);
                request.Content = content;

                var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data);
                }
                else
                {
                    return default;
                }
            }
        }



        private HttpContent CreateHttpContent(object content)
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

        public async Task<FileStackFileUploadResponse> Upload(string imageUrl)
        {
            string relativePath = string.Concat(StaticData.FileStackUrl, StaticData.FileStackUrl);

            var result = await PostAsync<FileStackFileUploadResponse>(relativePath, imageUrl, true);

            if (result is null || result.url is null) return new FileStackFileUploadResponse();

            return result;
        }

        public async Task<FileStackFileUploadResponse> Upload(byte[] imageFile)
        {
            string relativePath = string.Concat(StaticData.FileStackUrl, StaticData.FileStackUrl);

            var result = await PostAsync<FileStackFileUploadResponse>(relativePath, imageFile);

            if (result is null || result.url is null) return new FileStackFileUploadResponse();

            return result;
        }


        public async Task<FileStackFileUploadResponse> Upload(byte[] imageFile, string contentType)
        {
            string relativePath = string.Concat(StaticData.FileStackUrl, StaticData.FileStackUrl);

            var result = await PostAsync<FileStackFileUploadResponse>(relativePath, imageFile, contentType);

            if (result is null || result.url is null) return new FileStackFileUploadResponse();

            return result;
        }

        public async Task<string> GetContentType(string imagePath)
        {
            string contentType = MimeTypesMap.GetMimeType(imagePath);

            return await Task.FromResult(contentType);
        }
    }
}
