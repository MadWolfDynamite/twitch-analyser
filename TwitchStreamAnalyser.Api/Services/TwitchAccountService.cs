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

        public async Task<IEnumerable<TwitchAccount>> ListAsync()
        {
            return await _twitchAccountRepository.ListAsync();
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user)
        {
            return await _twitchAccountRepository.GetTwitchAccount(user);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user)
        {
            return await _twitchAccountRepository.GetTwitchChannel(user);
        }

        public async Task<IEnumerable<TwitchGame>> GetTwitchGame(string id)
        {
            return await _twitchAccountRepository.GetTwitchGame(id);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStream(string id)
        {
            return await _twitchAccountRepository.GetTwitchStream(id);
        }

        public async Task<int> GetTwitchClips(string id, string date)
        {
            return await _twitchAccountRepository.GetTwitchClips(id, date);
        }

        public async Task<int> GetTwitchFollowers(string id)
        {
            return await _twitchAccountRepository.GetTwitchFollowers(id);
        }
    }
}
