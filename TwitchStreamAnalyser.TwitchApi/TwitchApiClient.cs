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

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
           /* _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!_client.DefaultRequestHeaders.Contains("client-id"))
                _client.DefaultRequestHeaders.Add("client-id", clientId);

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
        }

        public async Task<IEnumerable<TwitchAccount>> GetTwitchAccountsAsync(string clientId, string token, string login = null)
        {
            string apiPath = string.IsNullOrWhiteSpace(login) ? "users" : $"users?login={login}";
            return await GetApiData<TwitchAccount>(apiPath, clientId, token);
        }

        public async Task<IEnumerable<TwitchChannel>> GetTwitchChannelsAsync(string login, string clientId, string token)
        {
            var apiPath = $"search/channels?query={login}&first=100";
            List<TwitchChannel> result = new List<TwitchChannel>();

            Stream stream;
            var channelData = new TwitchResponse<TwitchChannel>();
            var isSuccessfulResponse = false;

            do
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("client-id", clientId);

                var response = await _client.SendAsync(request);
                stream = await response.Content.ReadAsStreamAsync();

                isSuccessfulResponse = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    channelData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchChannel>>(stream);
                    result.AddRange(channelData.Data);

                    apiPath = $"search/channels?query={login}&first=100&after={channelData.Pagination.Cursor}";
                }

            } while (isSuccessfulResponse && !string.IsNullOrWhiteSpace(channelData.Pagination.Cursor));

            if (!isSuccessfulResponse)
            {
                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }

            return result;
        }

        public async Task<IEnumerable<TwitchGame>> GetTwitchGameAsync(string id, string clientId, string token)
        {
            string apiPath = $"games?id={id}";
            return await GetApiData<TwitchGame>(apiPath, clientId, token);
        }

        public async Task<IEnumerable<TwitchStream>> GetTwitchStreamsAsync(string id, string clientId, string token)
        {
            string apiPath = $"streams?user_id={id}";
            return await GetApiData<TwitchStream>(apiPath, clientId, token);
        }

        public async Task<int> GetTotalTwitchFollowersAsync(string id, string clientId, string token)
        {
            string apiPath = $"users/follows?to_id={id}";

            using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("client-id", clientId);

            var response = await _client.SendAsync(request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var streamData = StreamSerializer.DeserialiseJsonFromStream<TwitchFollowerResponse<TwitchFollower>>(stream);
                return streamData.Total;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public async Task<int> GetTotalTwitchClipsAsync(string id, string date, string clientId, string token)
        {
            int result = 0;
            var apiPath = $"clips?broadcaster_id={id}&first=100&started_at={date}";

            Stream stream;
            var clipData = new TwitchResponse<TwitchClip>();
            var isSuccessfulResponse = false;

            do
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("client-id", clientId);

                var response = await _client.SendAsync(request);
                stream = await response.Content.ReadAsStreamAsync();

                isSuccessfulResponse = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    clipData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchClip>>(stream);
                    result += clipData.Data.Count();

                    apiPath = $"clips?broadcaster_id={id}&first=100&started_at={date}&after={clipData.Pagination.Cursor}";
                }

            } while (isSuccessfulResponse && !string.IsNullOrWhiteSpace(clipData.Pagination.Cursor));

            if (!isSuccessfulResponse)
            {
                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }

            return result;
        }

        private async Task<IEnumerable<T>> GetApiData<T>(string path, string clientId, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("client-id", clientId);

            var response = await _client.SendAsync(request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var outputData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<T>>(stream);
                return outputData.Data;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }
    }
}
