using System;
using System.Collections.Generic;
using System.Globalization;
using DevChatter.DevStreams.Core.Model;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public static class ChannelMappings
    {
        public static ChannelViewModel ToChannelViewModel(this Channel src)
        {
            return new ChannelViewModel
            {
                Id = src.Id,
                Name = src.Name,
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                Uri = src.Uri,
                ScheduledStreamsCount = src.ScheduledStreams.Count
            };
        }

        public static void ApplyEditChanges(this Channel model,
            ChannelEditModel editModel)
        {
            model.Name = editModel.Name;
            model.Uri = editModel.Uri;
            model.CountryCode = editModel.CountryCode;
            model.TimeZoneId = editModel.TimeZoneId;
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
            };
        }
    }
}