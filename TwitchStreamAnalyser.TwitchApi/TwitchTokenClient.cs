using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.TwitchApi.Models;

namespace TwitchStreamAnalyser.TwitchApi
{
    public sealed class TwitchTokenClient
    {
        private static readonly TwitchTokenClient _instance;
        private static readonly HttpClient _client = new HttpClient();

        static TwitchTokenClient()
        {
            _instance = new TwitchTokenClient();
            _client.BaseAddress = new Uri("https://id.twitch.tv/");
        }
        private TwitchTokenClient() { }

        public static TwitchTokenClient GetClient()
        {
            return _instance;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);
            HttpResponseMessage response = await _client.GetAsync("oauth2/validate");

            return response.IsSuccessStatusCode;
        }

        public string GetAuthenticationUrl(string clientId, string redirectUrl)
        {
            string authUrl = $"{_client.BaseAddress}oauth2/authorize";

            authUrl += $"?client_id={clientId}";
            authUrl += $"&redirect_uri={redirectUrl}";
            authUrl += "&response_type=code";
            //authUrl += "&scope=user:read:email";

            return authUrl;
        }

        public async Task<TwitchToken> GetAccessTokenAsync(string clientId, string clientSecret, string code, string redirectUrl)
        {
            string apiPath = "oauth2/token";

            apiPath += $"?client_id={clientId}";
            apiPath += $"&client_secret={clientSecret}";
            apiPath += $"&code={code}";
            apiPath += "&grant_type=authorization_code";
            apiPath += $"&redirect_uri={redirectUrl}";

            var response = await _client.PostAsync(apiPath, null);
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
