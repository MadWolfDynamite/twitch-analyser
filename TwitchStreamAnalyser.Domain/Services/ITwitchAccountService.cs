using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Domain.Services
{
    public interface ITwitchAccountService
    {
        Task<IEnumerable<TwitchAccount>> ListAsync();

        Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user);

        Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user);

        Task<IEnumerable<TwitchGame>> GetTwitchGame(long id);

        Task<IEnumerable<TwitchStream>> GetTwitchStream(long id);

        Task<int> GetTwitchClips(long id, string date);

        Task<int> GetTwitchFollowers(long id);

        Task SendTwitchAnnoucement(long id, string message);
    }
}
