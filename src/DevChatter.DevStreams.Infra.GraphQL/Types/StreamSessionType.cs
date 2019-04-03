using DevChatter.DevStreams.Core.Model;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class StreamSessionType : ObjectGraphType<StreamSession>
    {
        public StreamSessionType()
        {
            Field(f => f.Id).Description("Stream Session unique identifier");
            Field(f => f.ChannelId);
            Field<InstantGraphType>("utcStartTime", 
                resolve: ctx => ctx.Source.UtcStartTime);
            Field<InstantGraphType>("utcEndTime",
                resolve: ctx => ctx.Source.UtcEndTime);
        }
    }
}