using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Repositories
{
    public interface ITwitchTokenRepository
    {
        string GetAuthenticationUrl(string client, string url);
    }
}
