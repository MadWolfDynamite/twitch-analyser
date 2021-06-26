using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Api.Resources
{
    public class TwitchStreamResource
    {
        public string User_Id { get; set; }

        public int Viewer_Count { get; set; }
        public string Started_At { get; set; }
    }
}
