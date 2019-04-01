using DevChatter.DevStreams.Core.Helpers;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class ChannelType : ObjectGraphType<Channel>
    {
        private IScheduledStreamService _scheduledStreamService;
        private string _timeZone = string.Empty;

        public ChannelType(IScheduledStreamService scheduledStreamService,
            IDataLoaderContextAccessor accessor)
        {
            _scheduledStreamService = scheduledStreamService;

            Field(f => f.Id).Description("Unique Channel Identifier");
            Field(f => f.Name).Description("The name of the streamer channel");
            Field(f => f.Uri).Description("The URL to access the channel");
            Field(f => f.CountryCode).Description("The country of residence of the streamer");
            Field(f => f.TimeZoneId).Description("The streamers local time zone");
            Field<ListGraphType<ScheduledStreamType>>("schedule",
                arguments: new QueryArguments(new QueryArgument<StringGraphType>
                {
                    Name = "timeZone"
                }),
                resolve: ctx =>
                {
                _timeZone = ctx.GetArgument<string>("timeZone");
                var loader = accessor.Context.GetOrAddCollectionBatchLoader<int, ScheduledStream>(
                    "GetScheduledStreams", GetScheduledStreams);

                    return loader.LoadAsync(ctx.Source.Id);
                });
        }

        private async Task<ILookup<int, ScheduledStream>> GetScheduledStreams(IEnumerable<int> channelIds)
        {
            var scheduleLookup = await _scheduledStreamService.GetChannelScheduleLookup(channelIds);

            if (!string.IsNullOrWhiteSpace(_timeZone))
            {
                foreach (var item in scheduleLookup)
                {
                    foreach (var stream in item)
                    {
                        stream.LocalStartTime = TimeZoneHelper.ConvertDateTimeToDifferentTimeZone(stream.LocalStartTime,
                            stream.TimeZoneId, _timeZone);
                        stream.LocalEndTime = TimeZoneHelper.ConvertDateTimeToDifferentTimeZone(stream.LocalEndTime,
                            stream.TimeZoneId, _timeZone);
                        stream.TimeZoneId = _timeZone;
                    }
                }
            }

            return scheduleLookup;
        }
    }
}
