using System.Collections.Generic;

namespace DevChatter.DevStreams.Web.Data.Model
{
    public class Streamer
    {
        public string Name { get; set; }
        public List<Stream> Streams { get; set; }
    }
}