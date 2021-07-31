using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Services
{
    public interface ITwitchTokenService
    {
        string GetAuthenticationUrl(string url);
    }
}
