using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly ITwitchTokenService _twitchTokenService;
        private readonly IMapper _mapper;

        public ValidationController(ITwitchTokenService twitchTokenService, IMapper mapper)
        {
            _twitchTokenService = twitchTokenService;
            _mapper = mapper;
        }

        [Route("AuthUrl")]
        public IActionResult GetAuthUrl(string client, string url)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(url))
                errors.Add("Url is required");
            if (string.IsNullOrWhiteSpace(client))
                errors.Add("Twitch Client Id is missing");

            if (errors.Count > 0)
                return BadRequest(errors);

            return Ok(_twitchTokenService.GetAuthenticationUrl(client, url));
        }

        [Route("Token")]
        public async Task<TwitchTokenResource> GetToken(string code, string state, string url)
        {
            var cleanedUrl = string.IsNullOrWhiteSpace(url) ? Request.GetEncodedUrl().Replace(Request.QueryString.Value, "") : url;

            var tokenData = await _twitchTokenService.GetTwitchToken("b6uwpcekra6xgg5yxw1kw473en4cly", "2lbxxa0rnzjqrl93kr3f8mycr1qr7d", code, cleanedUrl);
            var resource = _mapper.Map<TwitchToken, TwitchTokenResource>(tokenData);

            return resource;
        }
    }
}
