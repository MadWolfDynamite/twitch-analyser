using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class AccountController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly IMapper _mapper;

        public AccountController(ITwitchAccountService twitchAccountService, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync(string client, string token)
        {
            var accounts = await _twitchAccountService.ListAsync(client, token);
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync(string id, string client, string token)
        {
            var accounts = await _twitchAccountService.GetTwitchAccount(client, token, id);
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }
    }
}
