using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Client.Services
{
    public sealed class ApiClientService
    {
        public static readonly ApiClientService _instance;
        private static readonly HttpClient _client = new HttpClient();

        static ApiClientService()
        {
            _instance = new ApiClientService();
        }
        private ApiClientService() { }

        public static ApiClientService GetClient()
        {
            return _instance;
        }

        public void SetApiEndpoint(string endpoint)
        {
            if (_client.BaseAddress == null)
                _client.BaseAddress = new Uri(endpoint);
        }

        public async Task<bool> ValidateAuthToken(string token)
        {
            string apiPath = $"token/{token}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var validationData = StreamSerializer.DeserialiseJsonFromStream<bool>(stream);
                return validationData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<string> GetAuthenticationUrl(string client, string url)
        {
            string apiPath = "token/url";

            apiPath += $"?client={client}";
            apiPath += $"&url={url}";

            var response = await _client.GetAsync(apiPath);

            var stream = await response.Content.ReadAsStreamAsync();
            var content = await StreamSerializer.StreamToStringAsync(stream);

            if (response.IsSuccessStatusCode)
                return content;

            throw new Exception(content);
        }

        public async Task<TwitchToken> GetTwitchTokenAsync(string client, string secret, string code, string url)
        {
            string apiPath = "token/oauth";

            var data = new
            {
                ClientId = client,
                ClientSecret = secret,

                Token = code,
                RedirectUrl = url
            };

            var json = JsonConvert.SerializeObject(data);

            var request = new StringContent(json);
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(apiPath, request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenData = StreamSerializer.DeserialiseJsonFromStream<TwitchToken>(stream);
                return tokenData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }
    }
}
