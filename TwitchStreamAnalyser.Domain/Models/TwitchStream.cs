using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchStream
    {
        public string Id { get; set; }

        public string User_Id { get; set; }
        public string User_Name { get; set; }

        public string Game_Id { get; set; }
        public string Type { get; set; }

        public string Title { get; set; }
        public int Viewer_Count { get; set; }
        public string Started_At { get; set; }

        public string Language { get; set; }
        public string Thumbnail_Url { get; set; }

        public IEnumerable<string> Tag_Ids { get; set; }
    }
}
