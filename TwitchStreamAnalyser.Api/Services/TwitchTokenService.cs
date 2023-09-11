using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Services
{
    public class TwitchTokenService : ITwitchTokenService
    {
        private readonly ITwitchTokenRepository m_twitchTokenRepository;

        public TwitchTokenService(ITwitchTokenRepository twitchTokenRepository)
        {
            m_twitchTokenRepository = twitchTokenRepository;
        }

        public string GetAuthenticationUrl(string client, string url)
        {
            return m_twitchTokenRepository.GetAuthenticationUrl(client, url);
        }

        public string GetActiveClientId() 
        {
            return m_twitchTokenRepository.GetActiveClientId();
        }

        public async Task<TwitchToken> GetTwitchToken(string clientId, string clientSecret, string code, string redirectUrl)
        {
            return await m_twitchTokenRepository.GetTwitchToken(clientId, clientSecret, code, redirectUrl);
        }

        public async Task<bool> ValidateTwitchToken(string token)
        {
            return await m_twitchTokenRepository.ValidateTwitchToken(token);
        }

        public async Task<TwitchToken> RefreshTwitchToken(string clientId, string clientSecret, string token)
        {
            return await m_twitchTokenRepository.RefreshTwitchToken(clientId, clientSecret, token);
        }

        public void SetTwitchToken(string clientId, string token)
        {
            m_twitchTokenRepository.SetTwitchToken(clientId, token);
        }
    }
}
