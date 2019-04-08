using DevChatter.DevStreams.Core.Helpers;
using DevChatter.DevStreams.Core.Model;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class StreamSessionType : ObjectGraphType<StreamSession>
    {
        public StreamSessionType()
        {
            Field(f => f.Id).Description("Stream Session unique identifier");
            Field(f => f.ChannelId).Description("Channel Id for the session");
            Field<InstantGraphType>("utcStartTime", 
                resolve: ctx => ctx.Source.UtcStartTime);
            Field<InstantGraphType>("utcEndTime",
                resolve: ctx => ctx.Source.UtcEndTime);

            Field<DateTimeGraphType>("localStartTime", "The start time in the specified time zone (UTC if not given)",
                arguments: new QueryArguments(new QueryArgument<StringGraphType>
                {
                    Name = "timeZone"
                }),
                resolve: ctx =>
                {
                    var tz = ctx.GetArgument<string>("timeZone");
                    return (string.IsNullOrWhiteSpace(tz))
                        ? ctx.Source.UtcStartTime.ToDateTimeUtc()
                        : TimeZoneHelper.ConvertFromUtcToLocalTimeZone(ctx.Source.UtcStartTime, tz);

                });

            Field<DateTimeGraphType>("localEndTime", "The end time in the specified time zone (UTC if not given)",
                arguments: new QueryArguments(new QueryArgument<StringGraphType>
                {
                    Name = "timeZone"
                }),
                resolve: ctx =>
                {
                    var tz = ctx.GetArgument<string>("timeZone");
                    return (string.IsNullOrWhiteSpace(tz))
                        ? ctx.Source.UtcEndTime.ToDateTimeUtc()
                        : TimeZoneHelper.ConvertFromUtcToLocalTimeZone(ctx.Source.UtcEndTime, tz);

                });
        }
    }
}