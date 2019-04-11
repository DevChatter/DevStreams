using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL
{
    public class DevStreamsQuery : ObjectGraphType
    {
        public DevStreamsQuery(ICrudRepository repo)
        {
            Field<ListGraphType<ChannelType>>("channels",
                resolve: ctx => repo.GetAll<Channel>());

            Field<ChannelType>("channel",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id"
                }),
                resolve: ctx =>
                {
                    var id = ctx.GetArgument<int>("id");
                    return repo.Get<Channel>(id);
                });
        }
    }
}
