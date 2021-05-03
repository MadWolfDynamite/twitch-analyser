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
using TwitchStreamAnalyser.TwitchIntegration;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using TwitchStreamAnalyser.FileProcessing;

namespace TwitchStreamAnalyser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TwitchAppClient _clientSettings;

        private static readonly string appDataFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\WolfEyeAnalyser";

        public HomeController(ILogger<HomeController> logger, IOptions<TwitchAppClient> clientSettings)
        {
            _logger = logger;
            _clientSettings = clientSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrWhiteSpace(TwitchApiClient.ClientId))
            {
                TwitchApiClient.ClientId = _clientSettings.Id;

                TwitchValidationClient.ClientId = _clientSettings.Id;
                TwitchValidationClient.ClientSecret = _clientSettings.Secret;
            }

            if (!Directory.Exists(appDataFolder))
                Directory.CreateDirectory(appDataFolder);

            if (!System.IO.File.Exists($@"{appDataFolder}\UserSettings.json"))
                JsonFileProcessor.GenerateConfigurationFile();

            if (System.IO.File.Exists($@"{appDataFolder}\SavedToken.json"))
            {
                var tokenDetails = await JsonFileProcessor.LoadAccessTokenFile();

                var timeSinceValidation = DateTime.UtcNow.Subtract(tokenDetails.DateTimestamp);
                if (timeSinceValidation.TotalMinutes >= 30)
                {
                    if (await TwitchValidationClient.ValidateTokenAsync(tokenDetails.Token))
                    {
                        TwitchApiClient.SetAccessToken(tokenDetails.Token);
                        return RedirectToAction("Index", "TwitchData");
                    }

                    if (!String.IsNullOrWhiteSpace(tokenDetails.RefreshToken))
                    {
                        var apiResponse = await TwitchValidationClient.RefreshAccessToken(tokenDetails.RefreshToken);
                        if (apiResponse != null)
                        {
                            tokenDetails.Token = apiResponse.Access_Token;
                            tokenDetails.RefreshToken = apiResponse.Refresh_Token;

                            tokenDetails.DateTimestamp = DateTime.UtcNow;

                            JsonFileProcessor.SaveAccessTokenFile(tokenDetails);

                            TwitchApiClient.SetAccessToken(tokenDetails.Token);
                            return RedirectToAction("Index", "TwitchData");
                        }
                    }
                }
                else
                {
                    if (await TwitchValidationClient.ValidateTokenAsync(tokenDetails.Token))
                    {
                        TwitchApiClient.SetAccessToken(tokenDetails.Token);
                        return RedirectToAction("Index", "TwitchData");
                    }
                }
            }

            ViewData["AuthenticationUrl"] = TwitchValidationClient.GetAuthenticationUrl(Url.Action("Authentication", "TwitchData", null, Request.Scheme));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
