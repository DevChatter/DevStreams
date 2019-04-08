namespace DevChatter.DevStreams.Client.GraphQL.Models
{
    public class ChannelModel
    {
        // TODO: fill out the rest of the properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string CountryCode { get; set; }
        public string TimeZoneId { get; set; }
        public StreamSessionModel NextStream { get; set; }
    }
}
