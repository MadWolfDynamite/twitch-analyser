using System.ComponentModel.DataAnnotations;

namespace TwitchStreamAnalyser.Api.Resources
{
    public class SendAnnouncementResource
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
