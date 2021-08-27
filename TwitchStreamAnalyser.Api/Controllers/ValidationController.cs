using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly ITwitchTokenService _twitchTokenService;

        public ValidationController(ITwitchTokenService twitchTokenService)
        {
            _twitchTokenService = twitchTokenService;
        }

        [Route("AuthUrl")]
        public string GetAuthUrl(string client, string url)
        {
            return _twitchTokenService.GetAuthenticationUrl(client, url);
        }

        [Route("Token")]
        public string GetToken()
        {
            return "";
        }
    }
}
