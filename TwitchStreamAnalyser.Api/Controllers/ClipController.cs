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
    public class ClipController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;

        public ClipController(ITwitchAccountService twitchAccountService)
        {
            _twitchAccountService = twitchAccountService;
        }

        [HttpGet]
        public async Task<int> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();
            var user = accounts.First();

            var resource = await _twitchAccountService.GetTwitchClips(user.Id, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<int> GetAsync(string id, string date)
        {
            var resource = await _twitchAccountService.GetTwitchClips(id, date);

            return resource;
        }
    }
}
