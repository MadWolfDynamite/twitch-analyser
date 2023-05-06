using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchChannel
    {
        public string Broadcaster_Language { get; set; }

        public string Display_Name { get; set; }
        public string Id { get; set; }

        public string Game_Id { get; set; }
        public string Game_Name { get; set; }

        public bool Is_Live { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public string Thumbnail_Url { get; set; }
        public string Title { get; set; }
        public string Started_At { get; set; }
    }
}
