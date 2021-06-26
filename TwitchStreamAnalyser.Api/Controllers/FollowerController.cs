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
        public async Task<int> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();
            var resource = await _twitchAccountService.GetTwitchFollowers(accounts.First().Id);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<int> GetAsync(string id)
        {
            var resource = await _twitchAccountService.GetTwitchFollowers(id);
            return resource;
        }
    }
}
