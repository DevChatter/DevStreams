using System.ComponentModel.DataAnnotations;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public class ChannelSearchModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Uri { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        public bool IsLive { get; set; }

        public string ChannelLinkText => IsLive ? "Watch Live 🔴" : "Go To Channel";
    }
}