using DevChatter.DevStreams.Core.Model;
using NodaTime.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public static class ScheduledStreamMappings
    {
        private static readonly LocalTimePattern TimePattern =
            LocalTimePattern.CreateWithInvariantCulture("HH:mm");

        public static ScheduledStreamEditModel ToEditViewModel(this ScheduledStream src)
        {
            
            return new ScheduledStreamEditModel
            {
                Id = src.Id,
                DayOfWeek = src.DayOfWeek,
                LocalStartTime = TimePattern.Format(src.LocalStartTime),
                LocalEndTime = TimePattern.Format(src.LocalEndTime)
            };
        }

        public static void ApplyEditChanges(this ScheduledStream model, 
            ScheduledStreamEditModel viewModel)
        {
            var parsedStart = TimePattern.Parse(viewModel.LocalStartTime);
            var parsedEnd = TimePattern.Parse(viewModel.LocalEndTime);

            model.DayOfWeek = viewModel.DayOfWeek;
            model.LocalStartTime = parsedStart.Value;
            model.LocalEndTime = parsedEnd.Value;
        }

        public static List<ScheduledStreamViewModel> ToScheduledStreamViewModels(
            this Channel channel)
        {
            var viewModels = channel.ScheduledStreams
                .Select(x => x.ToViewModel(channel))
                .ToList();

            return viewModels;
        }

        public static ScheduledStreamViewModel ToViewModel(this ScheduledStream src,
            Channel channel)
        {
            return new ScheduledStreamViewModel
            {
                Id = src.Id,
                DayOfWeek = src.DayOfWeek,
                LocalStartTime = TimePattern.Format(src.LocalStartTime),
                LocalEndTime = TimePattern.Format(src.LocalEndTime),
                TimeZoneName = TZNames.GetNamesForTimeZone(channel.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                ChannelId = channel.Id
            };
        }
    }
}