using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain;
using TwitchStreamAnalyser.Domain.Models;
using TwitchStreamAnalyser.Domain.Services;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;
        private readonly IMapper _mapper;

        public ChannelController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();

            var channels = await _twitchAccountService.GetTwitchChannel(accounts.First().Login);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }

        [HttpGet("{user}")]
        public async Task<IEnumerable<TwitchChannelResource>> GetAsync(string user, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var channels = await _twitchAccountService.GetTwitchChannel(user);
            var resource = _mapper.Map<IEnumerable<TwitchChannel>, IEnumerable<TwitchChannelResource>>(channels);

            return resource;
        }
    }
}
