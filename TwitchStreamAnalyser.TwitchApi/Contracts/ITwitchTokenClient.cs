using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.TwitchApi.Contracts
{
    public interface ITwitchTokenClient
    {
        Task<bool> ValidateAccessTokenAsync(string token);

        string GenerateAuthenticationUrl(string clientId, string redirectUrl);

        Task<TwitchToken> GetNewAccessTokenAsync(string clientId, string clientSecret, string code, string redirectUrl);

        Task<TwitchToken> RefreshAccessToken(string clientId, string clientSecret, string token);
    }
}
