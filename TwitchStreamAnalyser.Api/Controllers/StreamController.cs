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
    public class StreamController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;
        private readonly IMapper _mapper;

        public StreamController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchStreamResource>> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();

            var streams = await _twitchAccountService.GetTwitchStream(long.Parse(accounts.First().Id));
            var resource = _mapper.Map<IEnumerable<TwitchStream>, IEnumerable<TwitchStreamResource>>(streams);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchStreamResource>> GetAsync(long id, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var streams = await _twitchAccountService.GetTwitchStream(id);
            var resource = _mapper.Map<IEnumerable<TwitchStream>, IEnumerable<TwitchStreamResource>>(streams);
            
            return resource;
        }
    }
}
