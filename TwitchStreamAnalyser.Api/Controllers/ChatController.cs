using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwitchStreamAnalyser.Api.Extensions;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain;
using TwitchStreamAnalyser.Domain.Services;
using TwitchStreamAnalyser.TwitchApi.Contracts;

namespace TwitchStreamAnalyser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ITwitchAccountService _twitchAccountService;
        private readonly ITwitchApiClient _twitchApiClient;

        public ChatController(ITwitchAccountService twitchAccountService, ITwitchApiClient twitchApiClient)
        {
            _twitchAccountService = twitchAccountService;
            _twitchApiClient = twitchApiClient;
        }

        [Route("Announce"), HttpPost]
        public async Task<IActionResult> SendAnnouncement([FromBody] SendAnnouncementResource resource, [FromHeader] Authentication authentication)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            _twitchApiClient.SetAuthentication(authentication.ClientId, authentication.AccessToken);

            await _twitchAccountService.SendTwitchAnnoucement(resource.UserId, resource.Message);
            return Ok();
        }
    }
}
