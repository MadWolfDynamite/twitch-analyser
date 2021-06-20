using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Services;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;

        public AccountController(ITwitchAccountService twitchAccountService)
        {
            _twitchAccountService = twitchAccountService;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchAccount>> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();
            return accounts;
        }
    }
}
