using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchAccessTokenModel
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
        public int Expires_In { get; set; }

        public List<string> Scope { get; set; }
        public string Token_Type { get; set; }
    }
}
