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

        private ApiClientService ApiClient
        {
            get 
            {
                if (HttpContext == null)
                    return new ApiClientService();

                var name = "apiClient";
                if (!HttpContext.Session.Keys.Contains(name))
                {
                    var client = new ApiClientService();
                    client.SetApiEndpoint(_clientSettings.ApiEndpoint);

                    HttpContext.Session.SetString(name, JsonConvert.SerializeObject(client));
                }
                    
                var json = HttpContext.Session.GetString(name);
                return JsonConvert.DeserializeObject<ApiClientService>(json);
            }
        }

        public DashboardController(IOptions<TwitchAppClient> clientSettings)
        {
            _clientSettings = clientSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            var streamData = await ApiClient.GetTwitchData(loadedConfig.Login, loadedConfig.NowPlayingFile);

            return View(streamData);
        }

        public async Task<IActionResult> RefreshStreamData()
        {
            try
            {
                var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
                var streamData = await ApiClient.GetTwitchData(loadedConfig.Login, loadedConfig.NowPlayingFile);

                var viewName = streamData.IsLive ? "_LiveInformationPartial" : "_OfflineInformationPartial";
                return PartialView(viewName, streamData);
            }
            catch
            {

                var redirectUrl = Url.Action("Authentication", "Dashboard", null, Request.Scheme);
                ViewData["AuthenticationUrl"] = await ApiClient.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

                return PartialView("_ReauthenticationPartial");
            }
        }

        public async Task<IActionResult> Authentication(string code, string scope, string state)
        {
            var cleanedUrl = Request.GetEncodedUrl().Replace(Request.QueryString.Value, "");
            var response = await ApiClient.GetTwitchTokenAsync(_clientSettings.Id, _clientSettings.Secret, code, cleanedUrl);

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
            ViewData["AuthenticationUrl"] = await ApiClient.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

            return View();
        }
    }
}
