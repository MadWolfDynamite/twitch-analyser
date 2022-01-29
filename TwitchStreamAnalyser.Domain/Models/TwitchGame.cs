using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchGame
    {
        public string Box_Art_Url { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
