using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Models;

namespace TwitchStreamAnalyser.TwitchIntegration
{
    public static class TwitchValidationClient
    {
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }

        private static readonly HttpClient client = new HttpClient();
        private static readonly string _apiUrl = "https://id.twitch.tv";

        public static async Task<bool> ValidateTokenAsync(string token)
        {

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
            HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/oauth2/validate");

            return response.IsSuccessStatusCode;
        }

        public static string GetAuthenticationUrl(string redirectUrl)
        {
            string authUrl = $"{_apiUrl}/oauth2/authorize";

            authUrl += $"?client_id={ClientId}";
            authUrl += $"&redirect_uri={redirectUrl}";
            authUrl += "&response_type=code";
            //authUrl += "&scope=user:read:email";

            return authUrl;
        }

        public static async Task<TwitchAccessTokenModel> GetAccessToken(string code, string redirectUrl)
        {
            TwitchAccessTokenModel result = null;

            string apiPath = $"{_apiUrl}/oauth2/token";

            apiPath += $"?client_id={ClientId}";
            apiPath += $"&client_secret={ClientSecret}";
            apiPath += $"&code={code}";
            apiPath += "&grant_type=authorization_code";
            apiPath += $"&redirect_uri={redirectUrl}";

            HttpResponseMessage response = await client.PostAsync(apiPath, null);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchAccessTokenModel>();

            return result;
        }

        public static async Task<TwitchAccessTokenModel> RefreshAccessToken(string token)
        {
            TwitchAccessTokenModel result = null;

            string apiPath = $"{_apiUrl}/oauth2/token";

            apiPath += $"?client_id={ClientId}";
            apiPath += $"&client_secret={ClientSecret}";
            apiPath += $"&refresh_token={token}";
            apiPath += "&grant_type=refresh_token";

            HttpResponseMessage response = await client.PostAsync(apiPath, null);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchAccessTokenModel>();

            return result;
        }
    }
}
