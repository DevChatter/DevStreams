using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class ChannelType : ObjectGraphType<Channel>
    {
        public ChannelType(IScheduledStreamService scheduledStreamService)
        {
            Field(f => f.Id).Description("Unique Channel Identifier");
            Field(f => f.Name).Description("The name of the streamer channel");
            Field(f => f.Uri).Description("The URL to access the channel");
            Field(f => f.CountryCode).Description("The country of residence of the streamer");
            Field(f => f.TimeZoneId).Description("The streamers local time zone");
            Field<ListGraphType<ScheduledStreamType>>("schedule",
                resolve: ctx => scheduledStreamService.GetChannelSchedule(ctx.Source.Id));
        }
    }
}
