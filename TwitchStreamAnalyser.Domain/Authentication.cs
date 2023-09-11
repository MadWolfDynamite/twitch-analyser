using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TwitchStreamAnalyser.Domain
{
    public class Authentication
    {
        [Required]
        [FromHeader(Name = "client-id")]
        public string ClientId { get; set; }

        [Required]
        [FromHeader(Name = "auth-token")]
        public string AccessToken { get; set; }
    }
}
