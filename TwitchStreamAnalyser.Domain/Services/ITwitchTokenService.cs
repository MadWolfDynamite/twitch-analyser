﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Domain.Services
{
    public interface ITwitchTokenService
    {
        string GetAuthenticationUrl(string client, string url);

        Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl);
    }
}
