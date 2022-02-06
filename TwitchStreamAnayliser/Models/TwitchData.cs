using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchData
    {
        private string _artUrl;

        public string Name { get; set; }
        public string AvatarUrl { get; set; }

        public int Viewers { get; set; }
        public string StreamStartDateTime { get; set; }
        public int Clips { get; set; }

        public int ChannelViews { get; set; }
        public int ChannelFollowers { get; set; }

        public string GameName { get; set; }
        public string GameArtUrl { 
            get { return _artUrl; } 
            set 
            { 
                _artUrl = value.Replace("{width}", "390").Replace("{height}", "540"); //Add explicit img size
            }
        }

        public bool IsLive { get; set; }

        public string NowPlaying { get; set; }
    }
}
