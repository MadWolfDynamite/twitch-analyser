using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain;
using TwitchStreamAnalyser.Domain.Services;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClipController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;

        public ClipController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
        }

        [HttpGet]
        public async Task<int> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();
            var user = accounts.First();

            var resource = await _twitchAccountService.GetTwitchClips(long.Parse(user.Id), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<int> GetAsync(long id, string date, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var resource = await _twitchAccountService.GetTwitchClips(id, date);
            return resource;
        }
    }
}
