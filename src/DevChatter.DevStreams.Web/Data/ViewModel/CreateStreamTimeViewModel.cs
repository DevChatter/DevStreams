using System.ComponentModel.DataAnnotations;
using DevChatter.DevStreams.Core.Model;
using NodaTime;
using NodaTime.Text;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public class CreateStreamTimeViewModel
    {
        private static readonly LocalTimePattern TimePattern =
            LocalTimePattern.CreateWithInvariantCulture("HH:mm");

        public string Country { get; set; }

        [Display(Name="Time Zone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choose your timezone.")]
        public string TimeZoneId { get; set; }

        [Display(Name="Day of Week")]
        [Required]
        public IsoDayOfWeek DayOfWeek { get; set; }

        [Display(Name="Start Time")]
        [Required]
        public string LocalStartTime { get; set; }

        [Display(Name="End Time")]
        [Required]
        public string LocalEndTime { get; set; }

        public StreamTime ToModel()
        {
            var parsedStart = TimePattern.Parse(LocalStartTime);
            var parsedEnd = TimePattern.Parse(LocalEndTime);
            var localStartTime = parsedStart.Value;
            var localEndTime = parsedEnd.Value;

            return new StreamTime
            {
                DayOfWeek = DayOfWeek,
                TimeZoneId = TimeZoneId,
                LocalStartTime = localStartTime,
                LocalEndTime = localEndTime,
            };
        }
    }
}