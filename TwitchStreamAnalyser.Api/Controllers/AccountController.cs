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
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync(string id)
        {
            var accounts = await _twitchAccountService.GetTwitchAccount(id);
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }
    }
}
