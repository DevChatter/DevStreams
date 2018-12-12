using System.ComponentModel.DataAnnotations;
using DevChatter.DevStreams.Web.Data.Model;
using NodaTime;
using NodaTime.Text;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public class CreateStreamTimeViewModel
    {
        private static readonly LocalTimePattern TimePattern =
            LocalTimePattern.CreateWithInvariantCulture("HH:mm");

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choose your timezone.")]
        public string TimeZoneId { get; set; }

        [Required]
        public IsoDayOfWeek DayOfWeek { get; set; }

        [Required]
        public string LocalTime { get; set; }

        public StreamTime ToModel()
        {
            var parseResult = TimePattern.Parse(LocalTime);
            var localTime = parseResult.Value;

            return new StreamTime
            {
                DayOfWeek = DayOfWeek,
                TimeZoneId = TimeZoneId,
                LocalTime = localTime
            };
        }
    }
}