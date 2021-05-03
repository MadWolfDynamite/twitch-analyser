using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchUserModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Display_Name { get; set; }

        public string Type { get; set; }
        public string Broadcaster_Type { get; set; }

        public string Description { get; set; }
        public string Profile_Image_Url { get; set; }
        public string Offline_Image_Url { get; set; }

        public int View_Count { get; set; }

        public string Email { get; set; }
    }

    public class TwitchUserModelContext
    {
        public List<TwitchUserModel> Data { get; set; } = new List<TwitchUserModel>();
    }
}
