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
        public async Task<IEnumerable<TwitchAccount>> ListAsync()
        {
            return await _apiClient.GetTwitchAccountsAsync();
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user)
        {
            return await _apiClient.GetTwitchAccountsAsync(user);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user)
        {
            return await _apiClient.GetTwitchChannelsAsync(user);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStream(string id)
        {
            return await _apiClient.GetTwitchStreamsAsync(id);
        }

        public async Task<int> GetTwitchClips(string id, string date)
        {
            return await _apiClient.GetTotalTwitchClipsAsync(id, date);
        }

        public async Task<int> GetTwitchFollowers(string id)
        {
            return await _apiClient.GetTotalTwitchFollowersAsync(id);
        }
    }
}
