using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Models;

namespace TwitchStreamAnalyser.FileProcessing
{
    public static class JsonFileProcessor
    {
        private static readonly string appDataFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\WolfEyeAnalyser";

        public static bool SaveAccessTokenFile(AccessTokenModel data)
        {
            using (StreamWriter file = File.CreateText($@"{appDataFolder}\SavedToken.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }

            return true;
        }

        public static async Task<AccessTokenModel> LoadAccessTokenFile()
        {
            string tokenFile = $@"{appDataFolder}\SavedToken.json";
            return JsonConvert.DeserializeObject<AccessTokenModel>(await File.ReadAllTextAsync(tokenFile));
        }

        public static bool GenerateConfigurationFile()
        {
            using (StreamWriter file = File.CreateText($@"{appDataFolder}\UserSettings.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, new UserConfigurationModel());
            }

            return true;
        }

        public static bool SaveConfigurationFile(UserConfigurationModel data)
        {
            using (StreamWriter file = File.CreateText($@"{appDataFolder}\UserSettings.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }

            return true;
        }

        public static async Task<UserConfigurationModel> LoadConfigurationFile()
        {
            string configFile = $@"{appDataFolder}\UserSettings.json";
            return JsonConvert.DeserializeObject<UserConfigurationModel>(await File.ReadAllTextAsync(configFile));
        }
    }
}
