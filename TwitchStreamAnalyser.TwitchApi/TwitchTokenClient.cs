using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public static TwitchTokenClient GetInstance()
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
    }
}
