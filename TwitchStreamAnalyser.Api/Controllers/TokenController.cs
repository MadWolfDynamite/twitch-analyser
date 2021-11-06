using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Api.Extensions;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITwitchTokenService _twitchTokenService;
        private readonly IMapper _mapper;

        public TokenController(ITwitchTokenService twitchTokenService, IMapper mapper)
        {
            _twitchTokenService = twitchTokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<bool> GetAsync()
        {
            var savedToken = _twitchTokenService.GetActiveClientId();
            return await _twitchTokenService.ValidateTwitchToken(savedToken);
        }

        [HttpGet("{Id}")]
        public async Task<bool> GetAsync(string Id)
        {
            return await _twitchTokenService.ValidateTwitchToken(Id);
        }

        [HttpPut]
        public IActionResult SetToken([FromBody] UpdateTokenResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            _twitchTokenService.SetTwitchToken(resource.ClientId, resource.Token);

            return Ok();
        }

        [Route("Url")]
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

        [Route("OAuth"),HttpPost]
        public async Task<TwitchTokenResource> GetToken([FromBody] SaveTokenResource resource)
        {
            var cleanedUrl = string.IsNullOrWhiteSpace(resource.RedirectUrl) ? Request.GetEncodedUrl().Replace(Request.QueryString.Value, "") : resource.RedirectUrl;

            var tokenData = await _twitchTokenService.GetTwitchToken(resource.ClientId, resource.ClientSecret, resource.Token, cleanedUrl);
            var tokenResource = _mapper.Map<TwitchToken, TwitchTokenResource>(tokenData);

            return tokenResource;
        }

        [Route("Refresh"),HttpPost]
        public async Task<TwitchTokenResource> RefreshToekn([FromBody] SaveTokenResource resource)
        {
            var tokenData = await _twitchTokenService.RefreshTwitchToken(resource.ClientId, resource.ClientSecret, resource.Token);
            var tokenResource = _mapper.Map<TwitchToken, TwitchTokenResource>(tokenData);

            return tokenResource;
        }
    }
}
