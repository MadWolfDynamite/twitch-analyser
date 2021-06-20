﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Domain.Repositories
{
    public interface ITwitchAccountRepository
    {
        Task<IEnumerable<TwitchAccount>> ListAsync();

        Task<IEnumerable<TwitchAccount>> GetTwitchAccount(string user);
    }
}