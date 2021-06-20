using System;
using System.Collections.Generic;
using System.Text;
using TwitchStreamAnalyser.TwitchApi;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly TwitchApiClient _apiClient;

        public BaseRepository()
        {
            _apiClient = TwitchApiClient.GetClient();
            _apiClient.SetAuthentication("b6uwpcekra6xgg5yxw1kw473en4cly", "mq6pl2rz8r64rlda7k0bok1dw2ojgu");
        }
    }
}
