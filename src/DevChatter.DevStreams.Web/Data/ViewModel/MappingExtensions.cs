using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using NodaTime.Text;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public static class MappingExtensions
    {
        private static readonly LocalTimePattern TimePattern =
            LocalTimePattern.CreateWithInvariantCulture("HH:mm");

        public static EditScheduledStream ToEditViewModel(this ScheduledStream src)
        {
            
            return new EditScheduledStream
            {
                Id = src.Id,
                DayOfWeek = src.DayOfWeek,
                LocalStartTime = TimePattern.Format(src.LocalStartTime),
                LocalEndTime = TimePattern.Format(src.LocalEndTime)
            };
        }

        public static void ApplyEditChanges(this ScheduledStream model, 
            EditScheduledStream viewModel)
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
                .Select(x => new ScheduledStreamViewModel
                {
                    Id = x.Id,
                    DayOfWeek = x.DayOfWeek,
                    LocalStartTime = TimePattern.Format(x.LocalStartTime),
                    LocalEndTime = TimePattern.Format(x.LocalEndTime),
                    TimeZoneName = TZNames.GetNamesForTimeZone(channel.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic
                })
                .ToList();

            return viewModels;
        }
    }
}