using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public class CreateStreamTimeViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choose your timezone.")]
        public string TimeZoneId { get; set; }

        [Required]
        public IsoDayOfWeek DayOfWeek { get; set; }

        [Required]
        public string LocalTime { get; set; }
    }
}