using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class GameController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;
        private readonly IMapper _mapper;

        public GameController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchGameResource>> GetAsync([FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var accounts = await _twitchAccountService.ListAsync();
            var channelData = await _twitchAccountService.GetTwitchChannel(accounts.First().Login);

            var gameData = await _twitchAccountService.GetTwitchGame(long.Parse(channelData.First().Game_Id));
            var resource = _mapper.Map<IEnumerable<TwitchGame>, IEnumerable<TwitchGameResource>>(gameData);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchGameResource>> GetAsync(long id, [FromHeader]Authentication authentication)
        {
            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            var gameData = await _twitchAccountService.GetTwitchGame(id);
            var resource = _mapper.Map<IEnumerable<TwitchGame>, IEnumerable<TwitchGameResource>>(gameData);

            return resource;
        }
    }
}
