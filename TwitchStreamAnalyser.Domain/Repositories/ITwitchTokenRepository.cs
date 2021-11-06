using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Domain.Repositories
{
    public interface ITwitchTokenRepository
    {
        string GetAuthenticationUrl(string client, string url);

        string GetActiveClientId();

        Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl);
        Task<bool> ValidateTwitchToken(string token);

        Task<TwitchToken> RefreshTwitchToken(string clientId, string clientSecret, string token);

        void SetTwitchToken(string clientId, string token);
    }
}
