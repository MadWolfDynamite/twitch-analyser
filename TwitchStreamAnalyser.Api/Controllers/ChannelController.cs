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
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();

            var channels = await _twitchAccountService.GetTwitchChannel(accounts.First().Login);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync(string id)
        {
            var channels = await _twitchAccountService.GetTwitchChannel(id);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }
    }
}
