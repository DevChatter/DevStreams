using DevChatter.DevStreams.Core.Model;
using NodaTime.Text;

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
    }
}