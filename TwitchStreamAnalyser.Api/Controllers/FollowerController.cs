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
    public class FollowerController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;

        public FollowerController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient)
        {
            _twitchAccountService = twitchAccountService ?? throw new ArgumentNullException(nameof(twitchAccountService));
            _twitchApiClient = twitchApiClient ?? throw new ArgumentNullException(nameof(twitchApiClient));
        }

        [HttpGet]
        public async Task<int> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();
            var resource = await _twitchAccountService.GetTwitchFollowers(long.Parse(accounts.First().Id));

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<int> GetAsync(long id, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var resource = await _twitchAccountService.GetTwitchFollowers(id);
            return resource;
        }
    }
}
