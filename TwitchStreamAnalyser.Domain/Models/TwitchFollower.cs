using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchFollower
    {
        public string From_Id { get; set; }
        public string From_Name { get; set; }

        public string To_Id { get; set; }
        public string To_Name { get; set; }

        public string Followed_At { get; set; }
    }
}
