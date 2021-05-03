using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class UserConfigurationModel
    {
        public string Login { get; set; } = "";
        public string NowPlayingFile { get; set; } = "";
    }
}
