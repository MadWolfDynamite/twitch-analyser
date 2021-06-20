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
        private readonly ITwitchAccountRepository _twitchAccountrepository;

        public TwitchAccountService(ITwitchAccountRepository twitchAccountRepository)
        {
            _twitchAccountrepository = twitchAccountRepository;
        }

        public async Task<IEnumerable<TwitchAccount>> ListAsync()
        {
            return await _twitchAccountrepository.ListAsync();
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user)
        {
            return await _twitchAccountrepository.GetTwitchAccount(user);
        }
    }
}
