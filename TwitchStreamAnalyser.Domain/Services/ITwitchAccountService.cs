using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Domain.Services
{
    public interface ITwitchAccountService
    {
        Task<IEnumerable<TwitchAccount>> ListAsync(string clientId, string token);

        Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string clientId, string token, string user);

        Task<IEnumerable<TwitchChannel>> GetTwitchChannel(string user, string clientId, string token);

        Task<IEnumerable<TwitchGame>> GetTwitchGame(string id, string clientId, string token);

        Task<IEnumerable<TwitchStream>> GetTwitchStream(string id, string clientId, string token);

        Task<int> GetTwitchClips(string id, string date, string clientId, string token);

        Task<int> GetTwitchFollowers(string id, string clientId, string token);
    }
}
