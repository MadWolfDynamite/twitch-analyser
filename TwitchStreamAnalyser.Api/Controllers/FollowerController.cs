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
    public class FollowerController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;

        public FollowerController(ITwitchAccountService twitchAccountService)
        {
            _twitchAccountService = twitchAccountService;
        }

        [HttpGet]
        public async Task<int> GetAsync(string client, string token)
        {
            var accounts = await _twitchAccountService.ListAsync(client, token);
            var resource = await _twitchAccountService.GetTwitchFollowers(accounts.First().Id, client, token);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<int> GetAsync(string id, string client, string token)
        {
            var resource = await _twitchAccountService.GetTwitchFollowers(id, client, token);
            return resource;
        }
    }
}
