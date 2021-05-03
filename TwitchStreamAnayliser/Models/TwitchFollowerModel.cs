using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchFollowerModel
    {
        public string From_Id { get; set; }
        public string From_Name { get; set; }

        public string To_Id { get; set; }
        public string To_Name { get; set; }

        public string Followed_At { get; set; }
    }

    public class TwitchFollowerModelContext
    {
        public int Total { get; set; }
        public List<TwitchFollowerModel> Data { get; set; } = new List<TwitchFollowerModel>();

        public TwitchPaginationModel Pagination { get; set; }
    }
}
