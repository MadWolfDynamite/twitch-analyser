using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.TwitchApi.Contracts;
using TwitchStreamAnalyser.TwitchApi.Enums;
using TwitchStreamAnalyser.TwitchApi.Models;

namespace TwitchStreamAnalyser.TwitchApi
{
    public sealed class TwitchApiClient : ITwitchApiClient
    {
        private readonly HttpClient m_client;

        public string ClientId { get; private set; }
        public string AccessToken { get; private set; }

        public TwitchApiClient(IHttpClientFactory httpClientFactory)
        {
            m_client = httpClientFactory.CreateClient("twitch-api");
        }

         public void SetAuthentication(string clientId, string token)
         {
            ClientId = clientId;
            AccessToken = token;

            m_client.DefaultRequestHeaders.Clear();

            m_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            m_client.DefaultRequestHeaders.Add("client-id", ClientId);
            m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

         public async Task<IEnumerable<TwitchAccount>> GetTwitchAccountsAsync(string login = null)
         {
             var queryParam = long.TryParse(login, out long _)
                 ? "id"
                 : "login";

             string apiPath = string.IsNullOrWhiteSpace(login) ? "users" : $"users?{queryParam}={login}";
             return await GetApiData<TwitchAccount>(apiPath);
         }

         public async Task<IEnumerable<TwitchChannel>> GetTwitchChannelsAsync(string login)
         {
             var apiPath = $"search/channels?query={login}&first=100";
             List<TwitchChannel> result = new();

             Stream stream;
             var channelData = new TwitchResponse<TwitchChannel>();
             var isSuccessfulResponse = false;

             do
             {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

                 request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                 request.Headers.Add("client-id", ClientId);

                 var response = await m_client.SendAsync(request);
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

         public async Task<IEnumerable<TwitchGame>> GetTwitchGameAsync(long gameId)
         {
             string apiPath = $"games?id={gameId}";
             return await GetApiData<TwitchGame>(apiPath);
         }

         public async Task<IEnumerable<TwitchStream>> GetTwitchStreamsAsync(long userId)
         {
             string apiPath = $"streams?user_id={userId}";
             return await GetApiData<TwitchStream>(apiPath);
         }

         public async Task<int> GetTwitchFollowerCountAsync(long userId)
         {
            string apiPath = $"users/follows?to_id={userId}";

            using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Headers.Add("client-id", ClientId);

            var response = await m_client.SendAsync(request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var streamData = StreamSerializer.DeserialiseJsonFromStream<TwitchFollowerResponse<TwitchFollower>>(stream);
                return streamData.Total;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
         }

         public async Task<int> GetTwitchClipCountAsync(long userId, string date)
         {
             int result = 0;
             var apiPath = $"clips?broadcaster_id={userId}&first=100&started_at={date}";

             Stream stream = null;
             var clipData = new TwitchResponse<TwitchClip>();
             var isSuccessfulResponse = false;

             do
             {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiPath);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                request.Headers.Add("client-id", ClientId);

                var response = await m_client.SendAsync(request);
                stream = await response.Content.ReadAsStreamAsync();

                isSuccessfulResponse = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    clipData = StreamSerializer.DeserialiseJsonFromStream<TwitchResponse<TwitchClip>>(stream);
                    result += clipData.Data.Count();

                    apiPath = $"clips?broadcaster_id={userId}&first=100&started_at={date}&after={clipData.Pagination.Cursor}";

                }

             } while (isSuccessfulResponse && !string.IsNullOrWhiteSpace(clipData.Pagination.Cursor));

             if (!isSuccessfulResponse)
             {
                 var content = await StreamSerializer.StreamToStringAsync(stream);
                 throw new Exception(content);
             }

             return result;
         }

        public async Task SendTwitchAnnouncementAsync(long userId, string message, AnnouncementColourScheme colourTheme)
        {
            var apiPath = "users";
            var currentUser = await GetApiData<TwitchAccount>(apiPath);

            apiPath = $"chat/announcements?broadcaster_id={userId}&moderator_id={currentUser.First().Id}";
            using var request = new HttpRequestMessage(HttpMethod.Post, apiPath);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Headers.Add("client-id", ClientId);

            var data = new
            {
                message,
                color = colourTheme.ToString().ToLower(),
            };

            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await m_client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private async Task<IEnumerable<T>> GetApiData<T>(string path)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Headers.Add("client-id", ClientId);

            var response = await m_client.SendAsync(request);
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
