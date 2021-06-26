using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchClip
    {
        public string Id { get; set; }

        public string Url { get; set; }
        public string Embed_Url { get; set; }

        public string Broadcaster_Id { get; set; }
        public string Broadcaster_Name { get; set; }

        public string Creator_Id { get; set; }
        public string Creator_Name { get; set; }

        public string Video_Id { get; set; }
        public string Game_Id { get; set; }

        public string Language { get; set; }
        public string Title { get; set; }
        public int View_Count { get; set; }

        public string Created_At { get; set; }
        public string Thumbnail_Url { get; set; }
    }
}
