using System;
using System.Collections.Generic;
using System.Text;
using TwitchStreamAnalyser.TwitchApi;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly TwitchApiClient _apiClient;
        protected readonly TwitchTokenClient _tokenClient;

        public BaseRepository()
        {
            _apiClient = TwitchApiClient.GetClient();
            _tokenClient = TwitchTokenClient.GetClient();
        }
    }
}
