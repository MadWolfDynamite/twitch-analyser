using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Services
{
    public class TwitchAccountService : ITwitchAccountService
    {
        private readonly ITwitchAccountRepository _twitchAccountRepository;

        public TwitchAccountService(ITwitchAccountRepository twitchAccountRepository)
        {
            _twitchAccountRepository = twitchAccountRepository;
        }

        public async Task<IEnumerable<TwitchAccount>> ListAsync(string clientId, string token)
        {
            return await _twitchAccountRepository.ListAsync(clientId, token);
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string clientId, string token, string user)
        {
            return await _twitchAccountRepository.GetTwitchAccount(clientId, token, user);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user, string clientId, string token)
        {
            return await _twitchAccountRepository.GetTwitchChannel(user, clientId, token);
        }

        public async Task<IEnumerable<TwitchGame>> GetTwitchGame(string id, string clientId, string token)
        {
            return await _twitchAccountRepository.GetTwitchGame(id, clientId, token);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStream(string id, string clientId, string token)
        {
            return await _twitchAccountRepository.GetTwitchStream(id, clientId, token);
        }

        public async Task<int> GetTwitchClips(string id, string date, string clientId, string token)
        {
            return await _twitchAccountRepository.GetTwitchClips(id, date, clientId, token);
        }

        public async Task<int> GetTwitchFollowers(string id, string clientId, string token)
        {
            return await _twitchAccountRepository.GetTwitchFollowers(id, clientId, token);
        }
    }
}
