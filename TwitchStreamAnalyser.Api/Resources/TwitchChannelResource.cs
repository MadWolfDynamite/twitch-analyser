namespace TwitchStreamAnalyser.Api.Resources
{
    public class TwitchChannelResource
    {
        public string Id { get; set; }
        public string Display_Name { get; set; }

        public string Title { get; set; }

        public string Game_Id { get; set; }

        public bool Is_Live { get; set; }
        public string Started_At { get; set; }
    }
}
