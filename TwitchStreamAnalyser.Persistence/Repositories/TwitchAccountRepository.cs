using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public class TwitchAccountRepository : BaseRepository, ITwitchAccountRepository
    {
        public TwitchAccountRepository(ITwitchApiClient apiClient, ITwitchTokenClient tokenClient) : base(apiClient, tokenClient) { }

        public async Task<IEnumerable<TwitchAccount>> ListAsync()
        {
            return await m_apiClient.GetTwitchAccountsAsync(string.Empty);
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user)
        {
            return await m_apiClient.GetTwitchAccountsAsync(user);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user)
        {
            return await m_apiClient.GetTwitchChannelsAsync(user);
        }

        public async Task<IEnumerable<TwitchGame>> GetTwitchGame(long id)
        {
            return await m_apiClient.GetTwitchGameAsync(id);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStream(long id)
        {
            return await m_apiClient.GetTwitchStreamsAsync(id);
        }

        public async Task<int> GetTwitchClips(long id, string date)
        {
            return await m_apiClient.GetTwitchClipCountAsync(id, date);
        }

        public async Task<int> GetTwitchFollowers(long id)
        {
            return await m_apiClient.GetTwitchFollowerCountAsync(id);
        }

        public async Task SendTwitchAnnoucement(long id, string message)
        {
            await m_apiClient.SendTwitchAnnouncementAsync(id, message, TwitchApi.Enums.AnnouncementColourScheme.Primary);
        }
    }
}
