﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using TwitchStreamAnalyser.Models;
using TwitchStreamAnalyser.TwitchIntegration;
using TwitchStreamAnalyser.Controllers;
using TwitchStreamAnalyser.FileProcessing;

namespace TwitchStreamAnalyser.Controllers
{
    public class TwitchDataController : Controller
    {
        private static readonly AccessTokenModel tokenDetails = new AccessTokenModel();

        public async Task<IActionResult> Index()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            var userApiResponse = await TwitchApiClient.GetUserDataAsync(loadedConfig.Login);

            if (userApiResponse == null)
                return RedirectToAction("Index", "Home");

            var selectedUser = userApiResponse.Data.First();

            ViewData["DisplayName"] = selectedUser.Display_Name;
            ViewData["UserAvatar"] = selectedUser.Profile_Image_Url;

            var channelApiResponse = await TwitchApiClient.GetChannelDataAsync(selectedUser.Login);

            if (channelApiResponse == null)
                return RedirectToAction("Index", "Home");

            var selectedChannel = channelApiResponse.Data.First(m => m.Id.Equals(selectedUser.Id));

            ViewData["IsLive"] = selectedChannel.Is_Live;

            var stats = new TwitchStatsModel()
            {
                ChannelViews = selectedUser.View_Count,
                ChannelFollowers = 0
            };

            if (selectedChannel.Is_Live)
            {
                var streamApiResponse = await TwitchApiClient.GetStreamDataAsync(selectedUser.Id);

                if (streamApiResponse == null)
                {
                    stats.Viewers = 1;
                    stats.StreamStartDateTime = selectedChannel.Started_At;
                    stats.Clips = 0;
                }
                else if (streamApiResponse.Data.Count < 1)
                {
                    ViewData["IsLive"] = false;
                }
                else
                {    
                    var selectedStream = streamApiResponse.Data.First();

                    stats.Viewers = selectedStream.Viewer_Count;
                    stats.StreamStartDateTime = selectedStream.Started_At;

                    var clipApiResponse = await TwitchApiClient.GetClipDataAsync(selectedUser.Id, selectedStream.Started_At);
                    stats.Clips = clipApiResponse != null ? clipApiResponse.Data.Count : 0;

                    if (System.IO.File.Exists(loadedConfig.NowPlayingFile))
                    {
                        stats.NowPlaying = await System.IO.File.ReadAllTextAsync(loadedConfig.NowPlayingFile);
                        stats.NowPlaying = stats.NowPlaying.Trim();
                    }
                }
            }

            var followerApiResponse = await TwitchApiClient.GetFollowerDataAsync(selectedUser.Id);
            if (followerApiResponse != null)
                stats.ChannelFollowers = followerApiResponse.Total;

            return View(stats);
        }

        public async Task<IActionResult> RefreshStreamData()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            var userApiResponse = await TwitchApiClient.GetUserDataAsync(loadedConfig.Login);

            if (userApiResponse != null)
            {
                var selectedUser = userApiResponse.Data.First();
                var channelApiResponse = await TwitchApiClient.GetChannelDataAsync(selectedUser.Login);

                if (channelApiResponse != null)
                {
                    var stats = new TwitchStatsModel()
                    {
                        ChannelViews = selectedUser.View_Count,
                        ChannelFollowers = 0
                    };

                    var followerApiResponse = await TwitchApiClient.GetFollowerDataAsync(selectedUser.Id);
                    if (followerApiResponse != null)
                        stats.ChannelFollowers = followerApiResponse.Total;

                    var selectedChannel = channelApiResponse.Data.First(m => m.Id.Equals(selectedUser.Id));

                    if (selectedChannel.Is_Live) 
                    {
                        var streamApiResponse = await TwitchApiClient.GetStreamDataAsync(selectedUser.Id);

                        if (streamApiResponse == null)
                        {
                            stats.Viewers = 1;
                            stats.StreamStartDateTime = selectedChannel.Started_At;
                            stats.Clips = 0;

                            return PartialView("_LiveInformationPartial", stats);
                        }
                        if  (streamApiResponse.Data.Count < 1)
                            return PartialView("_OfflineInformationPartial", stats);

                        var selectedStream = streamApiResponse.Data.First();

                        stats.Viewers = selectedStream.Viewer_Count;
                        stats.StreamStartDateTime = selectedStream.Started_At;

                        var clipApiResponse = await TwitchApiClient.GetClipDataAsync(selectedUser.Id, selectedStream.Started_At);
                        stats.Clips = clipApiResponse != null ? clipApiResponse.Data.Count : 0;

                        if (System.IO.File.Exists(loadedConfig.NowPlayingFile))
                        {
                            stats.NowPlaying = await System.IO.File.ReadAllTextAsync(loadedConfig.NowPlayingFile);
                            stats.NowPlaying = stats.NowPlaying.Trim();
                        }

                        return PartialView("_LiveInformationPartial", stats);
                    }

                    return PartialView("_OfflineInformationPartial", stats);
                }
            }

            ViewData["AuthenticationUrl"] = TwitchValidationClient.GetAuthenticationUrl(Url.Action("Authentication", "TwitchData", null, Request.Scheme));
            return PartialView("_ReauthenticationPartial");
        }

        public async Task<IActionResult> Authentication(string code, string scope, string state)
        {
            var cleanedUrl = Request.GetEncodedUrl().Replace(Request.QueryString.Value, "");
            var apiResponse = await TwitchValidationClient.GetAccessToken(code, cleanedUrl);

            if (apiResponse != null)
            {
                tokenDetails.Token = apiResponse.Access_Token;
                tokenDetails.RefreshToken = apiResponse.Refresh_Token;

                tokenDetails.DateTimestamp = DateTime.UtcNow;

                JsonFileProcessor.SaveAccessTokenFile(tokenDetails);

                return RedirectToAction("Index", "TwitchData");
            }

            ViewData["AuthenticationUrl"] = TwitchValidationClient.GetAuthenticationUrl(Url.Action("Authentication", "TwitchData", null, Request.Scheme));
            return View();
        }
    }
}
