using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public class TwitchTokenRepository : BaseRepository, ITwitchTokenRepository
    {
        public string GetAuthenticationUrl(string client, string url)
        {
            return _tokenClient.GetAuthenticationUrl(client, url);
        }

        public string GetActiveClientId()
        {
            return _apiClient.GetAuthentication();
        }

        public async Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl)
        {
            var tokenData = await _tokenClient.GetAccessTokenAsync(clientId, clientSecret, code, redirectUrl);
            _apiClient.SetAuthentication(clientId, tokenData.Access_Token);

            return tokenData;
        }

        public async Task<bool> ValidateTwitchToken(string token)
        {
            return await _tokenClient.ValidateTokenAsync(token);
        }

        public async Task<TwitchToken> RefreshTwitchToken(string clientId, string clientSecret, string token)
        {
            var tokenData = await _tokenClient.RefreshTwitchTokenAsync(clientId, clientSecret, token);
            _apiClient.SetAuthentication(clientId, tokenData.Access_Token);

            return tokenData;
        }

        public void SetTwitchToken(string clientId, string token)
        {
            _apiClient.SetAuthentication(clientId, token);
        }
    }
}
