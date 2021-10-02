using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.TwitchApi.Models;

namespace TwitchStreamAnalyser.TwitchApi
{
    public sealed class TwitchApiClient
    {
        private static readonly TwitchApiClient _instance;
        private static readonly HttpClient _client = new HttpClient();

        static TwitchApiClient() 
        {
            _instance = new TwitchApiClient();
            _client.BaseAddress = new Uri("https://api.twitch.tv/helix/");
        }
        private TwitchApiClient() { }

        public static TwitchApiClient GetClient()
        {
            return _instance;
        }

        public string GetAuthentication()
        {
            if (_client.DefaultRequestHeaders.Authorization == null)
                return "";

            var tokenData = _client.DefaultRequestHeaders.Authorization;
            return tokenData.ToString().Replace("Bearer ", "");
        }

        public void SetAuthentication(string clientId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!_client.DefaultRequestHeaders.Contains("client-id"))
                _client.DefaultRequestHeaders.Add("client-id", clientId);

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccountsAsync(string login = null)
        {
            string apiPath = String.IsNullOrWhiteSpace(login) ? "users" : $"users?login={login}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var accountData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchAccount>>(stream);
                return accountData.Data;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannelsAsync(string login)
        {
            string apiPath = $"search/channels?query={login}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var channelData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchChannel>>(stream);
                return channelData.Data;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStreamsAsync(string id)
        {
            string apiPath = $"streams?user_id={id}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var streamData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchStream>>(stream);
                return streamData.Data;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<int> GetTotalTwitchFollowersAsync(string id)
        {
            string apiPath = $"users/follows?to_id={id}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var streamData = StreamSerializer.DeserialiseJsonFromStream<TwitchFollowerResponse<TwitchFollower>>(stream);
                return streamData.Total;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<int> GetTotalTwitchClipsAsync(string id, string date)
        {
            int result = 0;
            string apiPath = $"clips?broadcaster_id={id}&first=100&started_at={date}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }

            var clipData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchClip>>(stream);
            do
            {
                result += clipData.Data.Count();
                apiPath = $"clips?broadcaster_id={id}&first=100&started_at={date}&after={clipData.Pagination.Cursor}";

                response = await _client.GetAsync(apiPath);
                stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    clipData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchClip>>(stream);

            } while (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(clipData.Pagination.Cursor));

            return result;
        }
    }
}
