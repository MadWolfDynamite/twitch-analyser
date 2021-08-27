using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Models
{
    public class TwitchToken
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
        public int Expires_In { get; set; }

        public List<string> Scope { get; set; }
        public string Token_Type { get; set; }
    }
}
