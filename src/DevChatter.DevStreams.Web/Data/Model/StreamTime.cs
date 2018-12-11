using System;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Pages.Streams;

namespace DevChatter.DevStreams.Web.Data.Model
{
    public class StreamTime
    {
        public DateTime Time { get; set; }
        public Location Location { get; set; }
    }
}