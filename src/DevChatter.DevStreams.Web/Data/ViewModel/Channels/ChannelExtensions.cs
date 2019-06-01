using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;
using System.Globalization;
using System.Linq;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public static class ChannelExtensions
    {
        public static ChannelIndexModel ToChannelIndexModel(this Channel src)
        {
            return new ChannelIndexModel
            {
                Id = src.Id,
                Name = src.Name,
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                Uri = src.Uri,
                ScheduledStreamsCount = src.ScheduledStreams.Count
            };
        }

        public static ChannelSearchModel ToChannelSearchModel(this Channel src)
        {
            return new ChannelSearchModel
            {
                Id = src.Id,
                Name = src.Name,
                Uri = src.Uri,
                Tags = string.Join(", ", src.Tags.Select(x => x.Name)),
                IsLive = false
            };
        }

        public static ChannelViewModel ToChannelViewModel(this Channel src)
        {
            return new ChannelViewModel
            {
                Id = src.Id,
                Name = src.Name,
                Uri = src.Uri,
                Description = src.Twitch?.Description,
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                ScheduledStreamsCount = src.ScheduledStreams.Count,
                Tags = string.Join(", ", src.Tags.Select(x => x.Name)),
                TwitchId = src.Twitch?.TwitchId,
                ImageUrl = src.Twitch?.ImageUrl ?? ""
            };
        }

        public static void ApplyEditChanges(this Channel model,
            ChannelEditModel editModel)
        {
            model.Name = editModel.Name;
            model.Uri = editModel.Uri;
            model.CountryCode = editModel.CountryCode;
            model.TimeZoneId = editModel.TimeZoneId;
            model.Tags = editModel.Tags.Select(x => new Tag { Id = x.Id }).ToList();
        }

        public static void ApplyTwitchChanges(this Channel model,
            TwitchChannel twitchChannel)
        {
            if (twitchChannel == null) return;

            twitchChannel.ChannelId = model.Id;
            model.Twitch = twitchChannel;
        }

        public static ChannelEditModel ToChannelEditModel(this Channel src)
        {
            return new ChannelEditModel
            {
                Id = src.Id,
                Name = src.Name,
                Uri = src.Uri,
                CountryCode = src.CountryCode,
                TimeZoneId = src.TimeZoneId,
                Tags = src.Tags.Select(x => x.ToViewModel()).ToList()
            };
        }
    }
}