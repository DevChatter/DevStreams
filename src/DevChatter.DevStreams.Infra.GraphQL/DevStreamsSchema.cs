using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL
{
    public class DevStreamsSchema : Schema
    {
        public DevStreamsSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<DevStreamsQuery>();
            RegisterTypes(new[] { typeof(IsoDayOfWeekGraphType), typeof(LocalTimeGraphType),
                typeof(InstantGraphType) });
        }
    }
}
