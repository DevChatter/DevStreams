using System.Collections.Generic;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL.Types;
using System.Linq;

namespace DevChatter.DevStreams.Infra.GraphQL
{
    public class DevStreamsQuery : ObjectGraphType
    {
        public DevStreamsQuery(IChannelRepository channelRepo)
        {
            Field<ListGraphType<ChannelType>>("channels",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<IdGraphType>>
                {
                    Name = "tagIds",
                    DefaultValue = new List<int>()
                }),
                resolve: ctx =>
                {
                    List<int> tagIds = ctx.GetArgument<List<int>>("tagIds");
                    if (tagIds.Any())
                    {
                        return channelRepo.GetChannelsByTagIds(tagIds.ToArray());
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
