using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class GameController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly IMapper _mapper;

        public GameController(ITwitchAccountService twitchAccountService, IMapper mapper)
        {
            _twitchAccountService = twitchAccountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TwitchGameResource>> GetAsync()
        {
            var accounts = await _twitchAccountService.ListAsync();
            var channelData = await _twitchAccountService.GetTwitchChannel(accounts.First().Login);

            var gameData = await _twitchAccountService.GetTwitchGame(channelData.First().Game_Id);
            var resource = _mapper.Map<IEnumerable<TwitchGame>, IEnumerable<TwitchGameResource>>(gameData);

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<TwitchGameResource>> GetAsync(string id)
        {
            var gameData = await _twitchAccountService.GetTwitchGame(id);
            var resource = _mapper.Map<IEnumerable<TwitchGame>, IEnumerable<TwitchGameResource>>(gameData);

            return resource;
        }
    }
}
