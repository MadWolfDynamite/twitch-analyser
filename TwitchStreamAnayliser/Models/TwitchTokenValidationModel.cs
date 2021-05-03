using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Models
{
    public class TwitchTokenValidationModel
    {
        public string Client_Id { get; set; }
        public string Login { get; set; }
        public List<String> Scopes { get; set; } = new List<string>();
        public string User_Id { get; set; }
    }
}
