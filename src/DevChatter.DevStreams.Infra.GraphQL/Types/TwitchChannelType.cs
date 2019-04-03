using DevChatter.DevStreams.Core.Model;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class TwitchChannelType : ObjectGraphType<TwitchChannel>
    {
        public TwitchChannelType()
        {
            Field(f => f.ChannelId).Description("The channel unique identifier");
            Field(f => f.TwitchId).Description("The Twitch ID of the channel");
            Field(f => f.TwitchName).Description("The name of the channel on Twitch");
            Field(f => f.IsAffiliate).Description("Is the channel a Twitch Affiliate?");
            Field(f => f.IsPartner).Description("Is the channel a Twitch Partner?");
        }
    }
}
