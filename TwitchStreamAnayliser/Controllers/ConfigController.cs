using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchStreamAnalyser.FileProcessing;
using TwitchStreamAnalyser.Models;

namespace TwitchStreamAnalyser.Controllers
{
    public class ConfigController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var loadedConfig = await JsonFileProcessor.LoadConfigurationFile();
            return View(loadedConfig);
        }

        public JsonResult UpdateConfigFile(string login, string file)
        {
            var updatedSettings = new UserConfigurationModel()
            {
                Login = String.IsNullOrWhiteSpace(login) ? "" : login.Trim().ToLower(),
                NowPlayingFile = String.IsNullOrWhiteSpace(file) ? "" : file.Trim().Replace('\\', '/')
            };

            bool isSuccessful = JsonFileProcessor.SaveConfigurationFile(updatedSettings);
            string responseMessage = isSuccessful ? "Settings successfully saved" : "Unable to save settings. Try again";

            return Json( new { success = isSuccessful, message = responseMessage });
        }
    }
}
