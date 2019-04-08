using DevChatter.DevStreams.Client.GraphQL.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Client.GraphQL
{
    public class ChannelGraphClient
    {
        private GraphQLClient _client;

        public ChannelGraphClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<ChannelModel> GetChannel(int id, string timeZone)
        {
            var query = new GraphQLRequest
            {
                Query = @"query channelQuery ($channelId: ID!, $tz: String)
                        { channel (id: $channelId)
                            { id name uri countryCode timeZoneId
                                nextStream
                                {
                                    localStartTime (timeZone: $tz)
                                    localEndTime (timeZone: $tz)
                                }
                            }
                        }",
                Variables = new { channelId = id, tz = timeZone }
            };

            var response = await _client.PostAsync(query);

            return response.GetDataFieldAs<ChannelModel>("channel");
        }
    }
}
