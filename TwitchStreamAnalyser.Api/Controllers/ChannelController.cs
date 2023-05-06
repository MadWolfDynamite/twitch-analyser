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
    public class ChannelController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;

        private readonly IMapper _mapper;

        public ChannelController(ITwitchAccountService twitchAccountService, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync(string client, string token)
        {
            var accounts = await _twitchAccountService.ListAsync(client, token);

            var channels = await _twitchAccountService.GetTwitchChannel(accounts.First().Login, client, token);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }

        [HttpGet("{user}")]
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync(string user, string client, string token)
        {
            var channels = await _twitchAccountService.GetTwitchChannel(user, client, token);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }
    }
}
