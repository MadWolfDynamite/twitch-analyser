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
    public class DashboardController : Controller
    {
        private readonly TwitchAppClient _clientSettings;

        private string ClientId
        {
            get
            {
                if (HttpContext == null)
                    return string.Empty;

                var name = "ClientId";
                if (!HttpContext.Session.Keys.Contains(name))
                {
                    var client = _clientSettings.Id;
                    HttpContext.Session.SetString(name, client);
                }

                return HttpContext.Session.GetString(name);
            }
        }

        private string AuthToken
        {
            get
            {
                if (HttpContext == null)
                    return string.Empty;

                var name = "AuthToken";
                if (!HttpContext.Session.Keys.Contains(name))
                {
                    var token = "";
                    HttpContext.Session.SetString(name, token);
                }

                return HttpContext.Session.GetString(name);
            }
        }

        public DashboardController(IOptions<TwitchAppClient> clientSettings)
        {
            _clientSettings = clientSettings.Value;
            ApiClientService.SetApiEndpoint(_clientSettings.ApiEndpoint);
        }

        public async Task<IActionResult> Index()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            var streamData = await ApiClientService.GetTwitchData(loadedConfig.Login, ClientId, AuthToken, loadedConfig.NowPlayingFile);

            return View(streamData);
        }

        public async Task<IActionResult> RefreshStreamData()
        {
            try
            {
                var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
                var streamData = await ApiClientService.GetTwitchData(loadedConfig.Login, ClientId, AuthToken, loadedConfig.NowPlayingFile);

                var viewName = streamData.IsLive ? "_LiveInformationPartial" : "_OfflineInformationPartial";
                return PartialView(viewName, streamData);
            }
            catch
            {

                var redirectUrl = Url.Action("Authentication", "Dashboard", null, Request.Scheme);
                ViewData["AuthenticationUrl"] = await ApiClientService.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

                return PartialView("_ReauthenticationPartial");
            }
        }

        public async Task<IActionResult> Authentication(string code, string scope, string state)
        {
            var cleanedUrl = Request.GetEncodedUrl().Replace(Request.QueryString.Value, "");
            var response = await ApiClientService.GetTwitchTokenAsync(_clientSettings.Id, _clientSettings.Secret, code, cleanedUrl);

            if (response != null)
            {
                var tokenData = new AccessTokenModel 
                { 
                    Token = response.Access_Token,
                    RefreshToken = response.Refresh_Token,

                    DateTimestamp = DateTime.UtcNow
                };

                JsonFileProcessor.SaveAccessTokenFile(tokenData);

                return RedirectToAction("Index", "Dashboard");
            }

            var redirectUrl = Url.Action("Authentication", "Dashboard", null, Request.Scheme);
            ViewData["AuthenticationUrl"] = await ApiClientService.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

            return View();
        }
    }
}
