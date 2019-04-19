using System.ComponentModel.DataAnnotations;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
	public class ChannelIndexModel
	{
		public int Id { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Uri")]
		public string Uri { get; set; }

		[Display(Name = "Scheduled Streams")]
		public int ScheduledStreamsCount { get; set; }

		[Display(Name = "Time Zone")]
		public string TimeZoneName { get; set; }


	}
}