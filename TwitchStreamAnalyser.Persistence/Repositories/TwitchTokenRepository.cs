using System;
using System.Collections.Generic;
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

        public async Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl)
        {
            return await _tokenClient.GetAccessTokenAsync(clientId, clientSecret, code, redirectUrl);
        }
    }
}
