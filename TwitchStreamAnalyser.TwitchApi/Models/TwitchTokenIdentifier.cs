namespace MadWolfOverlay.Web.Models.DTOs
{
    public class TwitchTokenIdentifier
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string AccessCode { get; set; }
        public string SourceUri { get; set; }
    }
}
