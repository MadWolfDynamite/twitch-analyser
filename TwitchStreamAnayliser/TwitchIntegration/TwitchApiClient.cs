using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Models;

namespace TwitchStreamAnalyser.TwitchIntegration
{
    public static class TwitchApiClient
    {
        public static string ClientId { get; set; }

        private static readonly HttpClient client = new HttpClient();
        private static readonly string _apiUrl = "https://api.twitch.tv/helix";

        public static void SetAccessToken(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!client.DefaultRequestHeaders.Contains("client-id"))
                client.DefaultRequestHeaders.Add("client-id", ClientId);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<TwitchUserModelContext> GetUserDataAsync(string login = null)
        {
            TwitchUserModelContext result = null;

            string apiPath = String.IsNullOrWhiteSpace(login) ? $"{_apiUrl}/users" : $"{_apiUrl}/users?login={login}";

            HttpResponseMessage response = await client.GetAsync(apiPath);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchUserModelContext>();

            return result;
        }

        public static async Task<TwitchChannelModelContext> GetChannelDataAsync(string login)
        {
            TwitchChannelModelContext result = null;

            HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/search/channels?query={login}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchChannelModelContext>();

            return result;
        }

        public static async Task<TwitchStreamModelContext> GetStreamDataAsync(string id)
        {
            TwitchStreamModelContext result = null;

            HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/streams?user_id={id}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchStreamModelContext>();

            return result;
        }

        public static async Task<TwitchFollowerModelContext> GetFollowerDataAsync(string id)
        {
            TwitchFollowerModelContext result = null;

            HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/users/follows?to_id={id}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchFollowerModelContext>();

            return result;
        }

        public static async Task<TwitchClipModelContext> GetClipDataAsync(string id, string date)
        {
            TwitchClipModelContext result = null;

            HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/clips?broadcaster_id={id}&first=100&&started_at={date}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<TwitchClipModelContext>();

            return result;
        }
    }
}
