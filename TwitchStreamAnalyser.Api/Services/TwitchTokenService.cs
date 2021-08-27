using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Services
{
    public class TwitchTokenService : ITwitchTokenService
    {
        private readonly ITwitchTokenRepository _twitchTokenRepository;

        public TwitchTokenService(ITwitchTokenRepository twitchTokenRepository)
        {
            _twitchTokenRepository = twitchTokenRepository;
        }

        public string GetAuthenticationUrl(string client, string url)
        {
            return _twitchTokenRepository.GetAuthenticationUrl(client, url);
        }
    }
}
