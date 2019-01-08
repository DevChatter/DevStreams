using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams
{
    public class ScheduledStreamViewModel
    {
        public int Id { get; set; }

        public int ChannelId { get; set; }

        [Display(Name = "Day of Week")]
        public IsoDayOfWeek DayOfWeek { get; set; }

        [Display(Name = "Start Time")]
        public string LocalStartTime { get; set; }

        [Display(Name = "End Time")]
        public string LocalEndTime { get; set; }

        [Display(Name = "Time Zone")]
        public string TimeZoneName { get; set; }
    }
}