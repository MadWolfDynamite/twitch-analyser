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
    }
}
