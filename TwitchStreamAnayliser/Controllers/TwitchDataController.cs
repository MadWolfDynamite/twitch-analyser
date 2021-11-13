using System;
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
using TwitchStreamAnalyser.FileProcessing;
using TwitchStreamAnalyser.Client.Services;
using Microsoft.Extensions.Options;

namespace TwitchStreamAnalyser.Controllers
{
    public class TwitchDataController : Controller
    {
        private readonly TwitchAppClient _clientSettings;
        private readonly ApiClientService _client;

        public TwitchDataController(IOptions<TwitchAppClient> clientSettings)
        {
            _clientSettings = clientSettings.Value;

            _client = ApiClientService.GetClient();
            _client.SetApiEndpoint(_clientSettings.ApiEndpoint);
        }

        public async Task<IActionResult> Index()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            var streamData = await _client.GetStreamData(loadedConfig.Login, loadedConfig.NowPlayingFile);

            return View(streamData);
        }

        public async Task<IActionResult> RefreshStreamData()
        {
            try
            {
                var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
                var streamData = await _client.GetStreamData(loadedConfig.Login, loadedConfig.NowPlayingFile);

                var viewName = streamData.IsLive ? "_LiveInformationPartial" : "_OfflineInformationPartial";
                return PartialView(viewName, streamData);
            }
            catch
            {

                var redirectUrl = Url.Action("Authentication", "TwitchData", null, Request.Scheme);
                ViewData["AuthenticationUrl"] = await _client.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

                return PartialView("_ReauthenticationPartial");
            }
        }

        public async Task<IActionResult> Authentication(string code, string scope, string state)
        {
            var cleanedUrl = Request.GetEncodedUrl().Replace(Request.QueryString.Value, "");
            var response = await _client.GetTwitchTokenAsync(_clientSettings.Id, _clientSettings.Secret, code, cleanedUrl);

            if (response != null)
            {
                var tokenData = new AccessTokenModel 
                { 
                    Token = response.Access_Token,
                    RefreshToken = response.Refresh_Token,

                    DateTimestamp = DateTime.UtcNow
                };

                JsonFileProcessor.SaveAccessTokenFile(tokenData);

                return RedirectToAction("Index", "TwitchData");
            }

            var redirectUrl = Url.Action("Authentication", "TwitchData", null, Request.Scheme);
            ViewData["AuthenticationUrl"] = await _client.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

            return View();
        }
    }
}
