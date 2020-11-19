using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csharp_sample
{
    public class BloxsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _customerName;

        public BloxsClient(string apiKey, string apiSecret, string customerName)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{customerName}.bloxs.io")
            };
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _customerName = customerName;
        }

        public async Task<HttpResponseMessage> GetAsync(string relativeUrl, CancellationToken cancellationToken)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long payloadSize = 0; // on a GET Request the payloadSize is 0
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(relativeUrl, UriKind.Relative),
                Headers = {
                    { "x-timestamp", timestamp.ToString() },
                    { HttpRequestHeader.Authorization.ToString(), GetAuthorizationToken(timestamp, payloadSize) },
                },
            };

            return await _httpClient.SendAsync(request, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsync(string relativeUrl, object requestModel, CancellationToken cancellationToken)
        {
            return await ExecuteJSONRequestAsync(relativeUrl, HttpMethod.Post, requestModel, cancellationToken);
        }

        public async Task<HttpResponseMessage> PutAsync(string relativeUrl, object requestModel, CancellationToken cancellationToken)
        {
            return await ExecuteJSONRequestAsync(relativeUrl, HttpMethod.Put, requestModel, cancellationToken);
        }

        public async Task<HttpResponseMessage> PatchAsync(string relativeUrl, object requestModel, CancellationToken cancellationToken)
        {
            return await ExecuteJSONRequestAsync(relativeUrl, HttpMethod.Patch, requestModel, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string relativeUrl, object requestModel, CancellationToken cancellationToken)
        {
            return await ExecuteJSONRequestAsync(relativeUrl, HttpMethod.Delete, requestModel, cancellationToken);
        }

        private async Task<HttpResponseMessage> ExecuteJSONRequestAsync(string relativeUrl, HttpMethod httpMethod, object requestModel, CancellationToken cancellationToken)
        {
            string json = "";
            if (requestModel != null)
            {
                json = JsonConvert.SerializeObject(requestModel);
            }

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long payloadSize = json.Length;
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(relativeUrl, UriKind.Relative),
                Headers = {
                    { "x-timestamp", timestamp.ToString() },
                    { HttpRequestHeader.Authorization.ToString(), GetAuthorizationToken(timestamp, payloadSize) },
                },
                Content = content,
            };

            return await _httpClient.SendAsync(request, cancellationToken);
        }

        // C# Token generation example
        private string GetAuthorizationToken(long timestamp, long payloadSize)
        {
            string apiKey = _apiKey;
            string apiSecret = _apiSecret;

            var tokenParts = apiKey + ":" + timestamp.ToString() + ":" + payloadSize.ToString();
            var hashedToken = GetSHA256Hash(tokenParts, apiSecret);
            return "bloxs " + apiKey + ":" + hashedToken;
        }

        private string GetSHA256Hash(string text, string key)
        {
            var encoding = new UTF8Encoding();

            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);

            byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
