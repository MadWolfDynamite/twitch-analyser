using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchChannelModel
    {
        public string Broadcaster_Language { get; set; }

        public string Display_Name { get; set; }
        public string Id { get; set; }

        public bool Is_Live { get; set; }
        public List<string> Tags_Ids { get; set; }

        public string Thumbnail_Url { get; set; }
        public string Title { get; set; }
        public string Started_At { get; set; }
    }

    public class TwitchChannelModelContext
    {
        public List<TwitchChannelModel> Data { get; set; } = new List<TwitchChannelModel>();
        public TwitchPaginationModel Pagination { get; set; }
    }
}
