using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public class TwitchTokenRepository : BaseRepository, ITwitchTokenRepository
    {
        public TwitchTokenRepository(ITwitchApiClient apiClient, ITwitchTokenClient tokenClient) : base(apiClient, tokenClient) { }

        public string GetAuthenticationUrl(string client, string url)
        {
            return m_tokenClient.GenerateAuthenticationUrl(client, url);
        }

        public string GetActiveClientId()
        {
            return m_apiClient.AccessToken;
        }

        public async Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl)
        {
            var tokenData = await m_tokenClient.GetNewAccessTokenAsync(clientId, clientSecret, code, redirectUrl);
            m_apiClient.SetAuthentication(clientId, tokenData.Access_Token);

            return tokenData;
        }

        public async Task<bool> ValidateTwitchToken(string token)
        {
            return await m_tokenClient.ValidateAccessTokenAsync(token);
        }

        public async Task<TwitchToken> RefreshTwitchToken(string clientId, string clientSecret, string token)
        {
            var tokenData = await m_tokenClient.RefreshAccessToken(clientId, clientSecret, token);
            m_apiClient.SetAuthentication(clientId, tokenData.Access_Token);

            return tokenData;
        }

        public void SetTwitchToken(string clientId, string token)
        {
            m_apiClient.SetAuthentication(clientId, token);
        }
    }
}
