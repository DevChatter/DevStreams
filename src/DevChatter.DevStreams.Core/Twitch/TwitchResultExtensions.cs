using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Twitch
{
    public static class TwitchResultExtensions
    {
        public static TwitchChannel ToTwitchChannelModel(this UserResultData src)
        {
            return new TwitchChannel
            {
                TwitchId = src.Id,
                TwitchName = src.Display_name,
                Description = src.Description,
                IsAffiliate = src.Broadcaster_type == TwitchConstants.AFFILIATE,
                IsPartner = src.Broadcaster_type == TwitchConstants.PARTNER,
                ImageUrl = src.Profile_image_url
            };
        }
    }
}