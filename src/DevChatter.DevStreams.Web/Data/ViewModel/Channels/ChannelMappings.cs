using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using TimeZoneNames;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public static class ChannelMappings
    {
        public static ChannelIndexModel ToChannelIndexModel(this Channel src)
        {
            return new ChannelIndexModel
            {
                Id = src.Id,
                Name = src.Name,
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                Uri = src.Uri,
                ScheduledStreamsCount = src.ScheduledStreamIds.Count
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
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                ScheduledStreamsCount = src.ScheduledStreamIds.Count,
                Tags = string.Join(", ", src.Tags.Select(x => x.Name))
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