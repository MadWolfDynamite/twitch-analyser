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
    }
}
