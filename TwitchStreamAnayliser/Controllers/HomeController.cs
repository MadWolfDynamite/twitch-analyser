using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwitchStreamAnalyser.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using TwitchStreamAnalyser.FileProcessing;
using TwitchStreamAnalyser.Client.Services;
using Microsoft.AspNetCore.Http;

namespace TwitchStreamAnalyser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

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

        private static readonly string appDataFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\WolfEyeAnalyser";

        public HomeController(ILogger<HomeController> logger, IOptions<TwitchAppClient> clientSettings)
        {
            _logger = logger;

            _clientSettings = clientSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            if (!Directory.Exists(appDataFolder))
                Directory.CreateDirectory(appDataFolder);

            if (!System.IO.File.Exists($@"{appDataFolder}\UserSettings.json"))
                JsonFileProcessor.GenerateConfigurationFile();

            if (System.IO.File.Exists($@"{appDataFolder}\SavedToken.json"))
            {
                var tokenDetails = await JsonFileProcessor.LoadAccessTokenFile();
                var validationResponse = await ApiClient.ValidateAuthToken(tokenDetails.Token);

                var timeSinceValidation = DateTime.UtcNow.Subtract(tokenDetails.DateTimestamp);
                if (timeSinceValidation.TotalMinutes >= 30 && !validationResponse)
                {
                    if (!String.IsNullOrWhiteSpace(tokenDetails.RefreshToken))
                    {
                        var response = await ApiClient.RefreshTwitchTokenAsync(_clientSettings.Id, _clientSettings.Secret, tokenDetails.RefreshToken);
                        if (response != null)
                        {
                            tokenDetails.Token = response.Access_Token;
                            tokenDetails.RefreshToken = response.Refresh_Token;

                            tokenDetails.DateTimestamp = DateTime.UtcNow;

                            JsonFileProcessor.SaveAccessTokenFile(tokenDetails);

                            await ApiClient.SetTwitchTokenAsync(_clientSettings.Id, tokenDetails.Token);
                            HttpContext.Session.SetString("apiClient", JsonConvert.SerializeObject(ApiClient));

                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }

                if (validationResponse)
                {
                    await ApiClient.SetTwitchTokenAsync(_clientSettings.Id, tokenDetails.Token);
                    HttpContext.Session.SetString("apiClient", JsonConvert.SerializeObject(ApiClient));

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            var redirectUrl = Url.Action("Authentication", "Dashboard", null, Request.Scheme);
            ViewData["AuthenticationUrl"] = await ApiClient.GetAuthenticationUrl(_clientSettings.Id, redirectUrl);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
