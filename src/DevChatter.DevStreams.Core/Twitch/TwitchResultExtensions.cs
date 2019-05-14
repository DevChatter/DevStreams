using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using NodaTime;

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

        public static List<ChannelLiveState> CreateChannelLiveStatesFromStreamResults(
            this StreamResult result, List<string> twitchIds)
        {
            List<ChannelLiveState> returnStat = twitchIds
                .Select(twitchId =>
                {
                    StreamResultData thisResult = result.Data.FirstOrDefault(x => x.User_id == twitchId);
                    return new ChannelLiveState
                    {
                        TwitchId = twitchId,
                        IsLive = thisResult != null,
                        StartedAt = thisResult?.Started_at ?? Instant.MinValue,
                        ViewerCount = thisResult?.Viewer_count ?? 0,
                    };
                })
                .ToList();
            return returnStat;
        }
    }
}