using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public class TwitchAccountRepository : BaseRepository, ITwitchAccountRepository
    {
        public async Task<IEnumerable<TwitchAccount>> ListAsync(string clientId, string token)
        {
            return await _apiClient.GetTwitchAccountsAsync(clientId, token);
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string clientId, string token, string user)
        {
            return await _apiClient.GetTwitchAccountsAsync(clientId, token, user);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user, string clientId, string token)
        {
            return await _apiClient.GetTwitchChannelsAsync(user, clientId, token);
        }

        public async Task<IEnumerable<TwitchGame>> GetTwitchGame(string id, string clientId, string token)
        {
            return await _apiClient.GetTwitchGameAsync(id, clientId, token);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStream(string id, string clientId, string token)
        {
            return await _apiClient.GetTwitchStreamsAsync(id, clientId, token);
        }

        public async Task<int> GetTwitchClips(string id, string date, string clientId, string token)
        {
            return await _apiClient.GetTotalTwitchClipsAsync(id, date, clientId, token);
        }

        public async Task<int> GetTwitchFollowers(string id, string clientId, string token)
        {
            return await _apiClient.GetTotalTwitchFollowersAsync(id, clientId, token);
        }
    }
}
