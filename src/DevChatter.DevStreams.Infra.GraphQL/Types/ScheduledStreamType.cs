using DevChatter.DevStreams.Core.Model;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class ScheduledStreamType : ObjectGraphType<ScheduledStream>
    {
        public ScheduledStreamType()
        {
            Field(f => f.Id).Description("Scheduled Stream unique identifier");
            Field(f => f.ChannelId).Description("Channel ID");
            Field(f => f.TimeZoneId).Description("The streamers local time zone");

            Field<IsoDayOfWeekGraphType>("dayOfWeek",
                "The day of the week",
                resolve: ctx => ctx.Source.DayOfWeek);
            Field<LocalTimeGraphType>("localStartTime",
                "The start time of the stream in the streamers local time zone",
                resolve: ctx => ctx.Source.LocalStartTime);
            Field<LocalTimeGraphType>("localEndTime",
                "The end time of the stream in the streamers local time zone",
                resolve: ctx => ctx.Source.LocalEndTime);
        }
    }
}
