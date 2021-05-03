using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchStreamModel
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

        public List<string> Tag_Ids { get; set; }
    }

    public class TwitchStreamModelContext
    {
        public List<TwitchStreamModel> Data { get; set; } = new List<TwitchStreamModel>();
        public TwitchPaginationModel Pagination { get; set; }
    }
}
