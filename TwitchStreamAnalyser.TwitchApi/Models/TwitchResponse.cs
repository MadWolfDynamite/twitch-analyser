using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.TwitchApi.Models
{
    public class TwitchResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public TwitchPagination Pagination { get; set; }
    }
}
