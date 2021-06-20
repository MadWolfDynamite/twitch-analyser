using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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

        private static T DeserialiseJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var reader = new StreamReader(stream);
            using var json = new JsonTextReader(reader);

            var serialiser = new JsonSerializer();
            var result = serialiser.Deserialize<T>(json);

            return result;
        }
        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            return content;
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
                var accountData = DeserialiseJsonFromStream<TwitchResponse<TwitchAccount>>(stream);
                return accountData.Data;
            }

            var content = await StreamToStringAsync(stream);
            throw new Exception(content);
        }
    }
}
