using DevChatter.DevStreams.Client.GraphQL.Models;
using GraphQL.Client;
using GraphQL.Common.Exceptions;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Client.GraphQL
{
    public class ChannelGraphClient
    {
        private readonly GraphQLClient _client;

        public ChannelGraphClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<ChannelModel> GetChannel(int id, string timeZone)
        {
            var query = new GraphQLRequest
            {
                Query = @"query channelQuery ($channelId: ID!, $tz: String!)
                        { channel (id: $channelId)
                            { id name uri countryCode timeZoneId
                                twitch
                                {
                                    twitchId twitchName isAffiliate isPartner
                                }
                                tags
                                {
                                    name
                                }
                                schedule (timeZone: $tz)
                                {
                                    dayOfWeek localStartTime localEndTime
                                }
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

            if (response.Errors is null)
            {
                return response.GetDataFieldAs<ChannelModel>("channel");
            }

            var error = response.Errors.First();
            throw new GraphQLException(
                new GraphQLError { Message = $"{error.Message}" }
                );
        }

        public async Task<ChannelModel> GetChannelFutureStreams(int id, string timeZone, int skip, int take)
        {
            var query = new GraphQLRequest
            {
                Query = @"query channelQuery ($channelId: ID!, $tz: String!, $skip: Int!, $take: Int!)
                        { channel (id: $channelId)
                            { id name uri countryCode timeZoneId
                                twitch
                                {
                                    twitchId twitchName isAffiliate isPartner
                                }
                                tags
                                {
                                    name
                                }
                                schedule (timeZone: $tz)
                                {
                                    dayOfWeek localStartTime localEndTime
                                }
                                futureStreams (skip: $skip, take: $take)
                                {
                                    localStartTime (timeZone: $tz)
                                    localEndTime (timeZone: $tz)
                                }
                            }
                        }",
                Variables = new { channelId = id, tz = timeZone, skip = skip, take = take }
            };

            var response = await _client.PostAsync(query);

            if (response.Errors is null)
            {
                return response.GetDataFieldAs<ChannelModel>("channel");
            }

            var error = response.Errors.First();
            throw new GraphQLException(
                new GraphQLError { Message = $"{error.Message}" }
                );
        }

        public async Task<List<ChannelModel>> GetChannels(string timeZone)
        {
            var query = new GraphQLRequest
            {
                Query = @"query channelQuery ($tz: String!)
                        { channels
                            { id name uri countryCode timeZoneId
                                twitch
                                {
                                    twitchId twitchName isAffiliate isPartner
                                }
                                tags
                                {
                                    name
                                }
                                schedule (timeZone: $tz)
                                {
                                    dayOfWeek localStartTime localEndTime
                                }
                                nextStream
                                {
                                    localStartTime (timeZone: $tz)
                                    localEndTime (timeZone: $tz)
                                }
                            }
                        }",
                Variables = new { tz = timeZone }
            };

            var response = await _client.PostAsync(query);
            if (response.Errors is null)
            {
                return response.GetDataFieldAs<List<ChannelModel>>("channels");
            }

            var error = response.Errors.First();
            throw new GraphQLException(
                new GraphQLError { Message = $"{error.Message}" }
                );
        }
    }
}
