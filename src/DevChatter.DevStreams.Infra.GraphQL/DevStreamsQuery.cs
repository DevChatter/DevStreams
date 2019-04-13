using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL
{
    public class DevStreamsQuery : ObjectGraphType
    {
        public DevStreamsQuery(IChannelRepository channelRepo)
        {
            Field<ListGraphType<ChannelType>>("channels",
                arguments: new QueryArguments(new QueryArgument<IdGraphType>
                {
                    Name = "tagId"
                }),
                resolve: ctx =>
                {
                    int tagId = ctx.GetArgument<int>("tagId");
                    if (tagId > 0)
                    {
                        return channelRepo.GetChannelsByTagIds(tagId);
                    }
                    return channelRepo.GetAll<Channel>();
                });

            Field<ChannelType>("channel",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id"
                }),
                resolve: ctx =>
                {
                    var id = ctx.GetArgument<int>("id");
                    return channelRepo.Get<Channel>(id);
                });
        }
    }
}
