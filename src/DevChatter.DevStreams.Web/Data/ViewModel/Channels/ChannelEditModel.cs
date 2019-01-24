using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public class ChannelEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string CountryCode { get; set; }
        public string TimeZoneId { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
        public string TagIdString { get; set; }
    }
}