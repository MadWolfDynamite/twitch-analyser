using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class AccessTokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime DateTimestamp { get; set; }
    }
}
