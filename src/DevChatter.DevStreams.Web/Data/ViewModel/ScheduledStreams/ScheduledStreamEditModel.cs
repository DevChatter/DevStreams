using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams
{
    public class ScheduledStreamEditModel
    {
        public int Id { get; set; }

        [Display(Name = "Day of Week")]
        [Required]
        public IsoDayOfWeek DayOfWeek { get; set; }

        [Display(Name = "Start Time")]
        [Required]
        public string LocalStartTime { get; set; }

        [Display(Name = "End Time")]
        [Required]
        public string LocalEndTime { get; set; }
    }
}