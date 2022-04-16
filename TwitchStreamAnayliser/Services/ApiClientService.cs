using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tools.StreamSerializer;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Models;

namespace TwitchStreamAnalyser.Client.Services
{
    public static class ApiClientService
    {
        private static readonly HttpClient _client = new HttpClient();

        public static void SetApiEndpoint(string endpoint)
        {
            if (_client.BaseAddress == null)
                _client.BaseAddress = new Uri(endpoint);
        }

        public static async Task<bool> ValidateAuthToken(string token)
        {
            string apiPath = $"token/{token}";

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var validationData = StreamSerializer.DeserialiseJsonFromStream<bool>(stream);
                return validationData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public static async Task<string> GetAuthenticationUrl(string client, string url)
        {
            string apiPath = "token/url";

            apiPath += $"?client={client}";
            apiPath += $"&url={url}";

            var response = await _client.GetAsync(apiPath);

            var stream = await response.Content.ReadAsStreamAsync();
            var content = await StreamSerializer.StreamToStringAsync(stream);

            if (response.IsSuccessStatusCode)
                return content;

            throw new Exception(content);
        }

        public static async Task<TwitchToken> GetTwitchTokenAsync(string client, string secret, string code, string url)
        {
            string apiPath = "token/oauth";

            var data = new
            {
                ClientId = client,
                ClientSecret = secret,

                Token = code,
                RedirectUrl = url
            };

            var json = JsonConvert.SerializeObject(data);

            var request = new StringContent(json);
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(apiPath, request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenData = StreamSerializer.DeserialiseJsonFromStream<TwitchToken>(stream);
                return tokenData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public static async Task SetTwitchTokenAsync(string client, string token)
        {
            string apiPath = "token";

            var data = new
            {
                ClientId = client,
                Token = token
            };

            var json = JsonConvert.SerializeObject(data);

            var request = new StringContent(json);
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PutAsync(apiPath, request);

            if (!response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }
        }

        public static async Task<TwitchToken> RefreshTwitchTokenAsync(string client, string secret, string token)
        {
            string apiPath = "token/refresh";

            var data = new
            {
                ClientId = client,
                ClientSecret = secret,

                Token = token
            };

            var json = JsonConvert.SerializeObject(data);

            var request = new StringContent(json);
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(apiPath, request);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenData = StreamSerializer.DeserialiseJsonFromStream<TwitchToken>(stream);
                return tokenData;
            }

            var content = await StreamSerializer.StreamToStringAsync(stream);
            throw new Exception(content);
        }

        public static async Task<TwitchData> GetTwitchData(string user, string client, string token, string musicFile = "")
        {
            var resource = new TwitchData();

            string apiPath = GetApiPath($"account/{user}", client, token);

            var response = await _client.GetAsync(apiPath);
            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }

            var users = StreamSerializer.DeserialiseJsonFromStream<IEnumerable<TwitchAccountResource>>(stream);
            var userData = string.IsNullOrWhiteSpace(user) ? users.FirstOrDefault() : users.FirstOrDefault(u => u.Login.Equals(user));

            resource.Name = userData.Display_Name;
            resource.AvatarUrl = userData.Profile_Image_Url;

            resource.ChannelViews = userData.View_Count;

            apiPath = GetApiPath($"channel/{userData.Login}", client, token);

            response = await _client.GetAsync(apiPath);
            stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamSerializer.StreamToStringAsync(stream);
                throw new Exception(content);
            }

            var channelData = StreamSerializer.DeserialiseJsonFromStream<IEnumerable<TwitchChannelResource>>(stream).FirstOrDefault(c => c.Id.Equals(userData.Id));

            resource.IsLive = channelData.Is_Live;
            resource.StreamStartDateTime = channelData.Started_At;

            if (resource.IsLive)
            {
                apiPath = GetApiPath($"stream/{userData.Id}", client, token);

                response = await _client.GetAsync(apiPath);
                stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    var streamList = StreamSerializer.DeserialiseJsonFromStream<IEnumerable<TwitchStreamResource>>(stream);
                    resource.IsLive = streamList.Count() > 0;

                    if (resource.IsLive)
                    {
                        var streamData = streamList.FirstOrDefault(s => s.User_Id.Equals(userData.Id));

                        resource.Viewers = streamData.Viewer_Count;
                        resource.StreamStartDateTime = streamData.Started_At;

                        apiPath = $"clip/{userData.Id}";
                        apiPath += $"?date={streamData.Started_At}";
                        apiPath += $"&client={client}";
                        apiPath += $"&token={token}";

                        response = await _client.GetAsync(apiPath);
                        stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var clipCount = StreamSerializer.DeserialiseJsonFromStream<int>(stream);
                            resource.Clips = clipCount;
                        }
                        else { resource.Clips = 0; }
                    }
                }
            }

            apiPath = GetApiPath($"game/{channelData.Game_Id}", client, token);

            response = await _client.GetAsync(apiPath);
            stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var gameData = StreamSerializer.DeserialiseJsonFromStream<IEnumerable<TwitchGameResource>>(stream).FirstOrDefault(g => g.Id.Equals(channelData.Game_Id));

                resource.GameName = gameData.Name;
                resource.GameArtUrl = gameData.Box_Art_Url;
            }
            else 
            {
                resource.GameName = "--";
                resource.GameArtUrl = "";
            }

            apiPath = GetApiPath($"follower/{userData.Id}", client, token);

            response = await _client.GetAsync(apiPath);
            stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                var followCount = StreamSerializer.DeserialiseJsonFromStream<int>(stream);
                resource.ChannelFollowers = followCount;
            }
            else { resource.ChannelFollowers = 0; }

            if (File.Exists(musicFile))
            {
                var songTitle = await File.ReadAllTextAsync(musicFile);
                resource.NowPlaying = songTitle.Trim();
            }

            return resource;
        }

        private static string GetApiPath(string path, string client, string token)
        {
            var apiPath = path;
            apiPath += $"?client={client}";
            apiPath += $"&token={token}";

            return apiPath;
        }
    }
}
