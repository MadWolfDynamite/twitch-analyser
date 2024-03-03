using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.TwitchApi
{
    public sealed class TwitchTokenClient : ITwitchTokenClient
    {
        private readonly HttpClient m_client;

        public TwitchTokenClient(IHttpClientFactory httpClientFactory)
        {
            m_client = httpClientFactory.CreateClient("twitch-oauth");
        }

        public async Task<bool> ValidateAccessTokenAsync(string token)
        {
            m_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
            HttpResponseMessage response = await m_client.GetAsync("oauth2/validate");

            return response.IsSuccessStatusCode;
        }

        public string GenerateAuthenticationUrl(string clientId, string redirectUrl)
        {
            string authUrl = $"{m_client.BaseAddress}oauth2/authorize";

            authUrl += $"?client_id={clientId}";
            authUrl += $"&redirect_uri={redirectUrl}";
            authUrl += "&response_type=code";

            return authUrl;
        }

        public async Task<TwitchToken> GetNewAccessTokenAsync(string clientId, string clientSecret, string code, string redirectUrl)
        {
            string apiPath = "oauth2/token";

            apiPath += $"?client_id={clientId}";
            apiPath += $"&client_secret={clientSecret}";
            apiPath += $"&code={code}";
            apiPath += "&grant_type=authorization_code";
            apiPath += $"&redirect_uri={redirectUrl}";

            var response = await m_client.PostAsync(apiPath, null);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenData = StreamSerializer.DeserialiseJsonFromStream<TwitchToken>(stream);
                return tokenData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<TwitchToken> RefreshAccessToken(string clientId, string clientSecret, string token)
        {
            string apiPath = "oauth2/token";

            apiPath += $"?client_id={clientId}";
            apiPath += $"&client_secret={clientSecret}";
            apiPath += $"&refresh_token={token}";
            apiPath += "&grant_type=refresh_token";

            var response = await m_client.PostAsync(apiPath, null);
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
