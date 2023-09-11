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
    public class AccountController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;
        private readonly IMapper _mapper;

        public AccountController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }

        [HttpGet("{login}")]
        public async Task<IEnumerable<TwitchAccountResource>> GetAsync(string login, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.GetTwitchAccount(login);
            var resource = _mapper.Map<IEnumerable<TwitchAccount>, IEnumerable<TwitchAccountResource>>(accounts);

            return resource;
        }
    }
}
