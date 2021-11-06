using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchStreamAnalyser.Api.Resources
{
    public class SaveTokenResource
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public string Token { get; set; }
        [Url]
        public string RedirectUrl { get; set; }
    }
}
