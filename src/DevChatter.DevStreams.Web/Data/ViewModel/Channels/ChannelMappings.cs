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
    }
}