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
    public class StreamController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;

        private readonly IMapper _mapper;

        public StreamController(ITwitchAccountService twitchAccountService, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchStreamResource>> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();

            var streams = await _twitchAccountService.GetTwitchStream(accounts.First().Id);
            var resource = _mapper.Map<IEnumerable<TwitchStream>, IEnumerable<TwitchStreamResource>>(streams);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchStreamResource>> GetAsync(string id)
        {
            var streams = await _twitchAccountService.GetTwitchStream(id);
            var resource = _mapper.Map<IEnumerable<TwitchStream>, IEnumerable<TwitchStreamResource>>(streams);
            
            return resource;
        }
    }
}
