using System;
using System.Collections.Generic;
using System.Text;
using TwitchStreamAnalyser.TwitchApi;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ITwitchApiClient m_apiClient;
        protected readonly ITwitchTokenClient m_tokenClient;

        public BaseRepository(ITwitchApiClient apiClient, ITwitchTokenClient tokenClient)
        {
            m_apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            m_tokenClient = tokenClient ?? throw new ArgumentNullException(nameof(tokenClient));
        }
    }
}
