using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.TwitchApi.Models
{
    public class TwitchFollowerResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
        public TwitchPagination Pagination { get; set; }
    }
}
