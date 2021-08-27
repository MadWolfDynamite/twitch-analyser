using System;
using System.Collections.Generic;
using System.Text;
using TwitchStreamAnalyser.Domain.Repositories;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public class TwitchTokenRepository : BaseRepository, ITwitchTokenRepository
    {
        public string GetAuthenticationUrl(string client, string url)
        {
            return _tokenClient.GetAuthenticationUrl(client, url);
        }
    }
}
