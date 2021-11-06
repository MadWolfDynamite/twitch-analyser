using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class StreamDetail
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }

        public int Viewers { get; set; }
        public string StreamStartDateTime { get; set; }
        public int Clips { get; set; }

        public int ChannelViews { get; set; }
        public int ChannelFollowers { get; set; }

        public bool IsLive { get; set; }

        public string NowPlaying { get; set; }
    }
}
