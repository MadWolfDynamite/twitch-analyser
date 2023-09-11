using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.TwitchApi.Enums;

namespace TwitchStreamAnalyser.TwitchApi.Contracts
{
    public interface ITwitchApiClient
    {
        string ClientId { get; }
        string AccessToken { get; }

        void SetAuthentication(string clientId, string token);

        Task<IEnumerable<TwitchAccount>> GetTwitchAccountsAsync(string login);
        Task<IEnumerable<TwitchChannel>> GetTwitchChannelsAsync(string login);
        Task<IEnumerable<TwitchGame>> GetTwitchGameAsync(long gameId);

        Task<IEnumerable<TwitchStream>> GetTwitchStreamsAsync(long userId);

        Task<int> GetTwitchFollowerCountAsync(long userId);
        Task<int> GetTwitchClipCountAsync(long userId, string date);

        Task SendTwitchAnnouncementAsync(long userId, string message, AnnouncementColourScheme colourTheme);
    }
}
