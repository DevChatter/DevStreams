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
        private readonly IScheduledStreamService _scheduledStreamService;
        private readonly ITagService _tagService;
        private readonly IStreamSessionService _dapperSessionLookup;
        private string _timeZone = string.Empty;
        private int _skip = 0;
        private int _take = 0;

        public ChannelType(IScheduledStreamService scheduledStreamService,
            ITagService tagService, IStreamSessionService dapperSessionLookup, 
            ITwitchChannelService twitchChannelService,
            IDataLoaderContextAccessor accessor)
        {
            _scheduledStreamService = scheduledStreamService;
            _tagService = tagService;
            _dapperSessionLookup = dapperSessionLookup;

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
            Field<ListGraphType<TagType>>("tags", "The subjects that the channel is tagged with",
                resolve: ctx =>
                {
                    var loader = accessor.Context.GetOrAddCollectionBatchLoader<int, Tag>(
                        "GetChannelTags", GetChannelTags);

                    return loader.LoadAsync(ctx.Source.Id);
                });
            Field<TwitchChannelType>("twitch", "Details of the channel on Twitch",
                resolve: ctx =>
                {
                    var loader = accessor.Context.GetOrAddBatchLoader<int, TwitchChannel>(
                        "GetTwitchChannel", twitchChannelService.GetTwitchChannel);

                    return loader.LoadAsync(ctx.Source.Id);
                });
            Field<StreamSessionType>("nextStream", "The times of the channels next stream",
                resolve: ctx =>
                {
                    var loader = accessor.Context.GetOrAddBatchLoader<int, StreamSession>(
                        "GetChannelNextStream", dapperSessionLookup.GetChannelNextStreamLookup);

                    return loader.LoadAsync(ctx.Source.Id);
                });

            Field<ListGraphType<StreamSessionType>>("futureStreams", "The channels future stream",
                arguments: new QueryArguments(new QueryArgument<IntGraphType>
                {
                    Name = "skip"
                }, 
                new QueryArgument<IntGraphType>
                {
                    Name = "take"
                }),
                resolve: ctx =>
                {
                    _skip = ctx.GetArgument<int>("skip");
                    _take = ctx.GetArgument<int>("take");
                    var loader = accessor.Context.GetOrAddCollectionBatchLoader<int, StreamSession>(
                        "GetChannelFutureStreams", GetChannelFutureStreams);

                    return loader.LoadAsync(ctx.Source.Id);
                });
        }

        private async Task<ILookup<int, StreamSession>> GetChannelFutureStreams(IEnumerable<int> channelIds)
        {
            var futureStreams = await _dapperSessionLookup.GetChannelFutureStreamsLookup(channelIds);

            if (_take > 0)
            {
                var streamSessions = new List<StreamSession>();

                foreach (var group in futureStreams)
                {
                    streamSessions.AddRange(group.Skip(_skip).Take(_take));
                }

                return streamSessions.ToLookup(c => c.ChannelId);
            }

            return futureStreams;
        }

        private async Task<ILookup<int, Tag>> GetChannelTags(IEnumerable<int> channelIds)
        {
            var channelTagLookup = await _tagService.GetChannelTagsLookup(channelIds);
            return channelTagLookup;
        }

        private async Task<ILookup<int, ScheduledStream>> GetScheduledStreams(IEnumerable<int> channelIds)
        {
            var scheduleLookup = await _scheduledStreamService.GetChannelScheduleLookup(channelIds);

            if (string.IsNullOrWhiteSpace(_timeZone)) return scheduleLookup;

            var streamsNotInZone = scheduleLookup
                .SelectMany(x => x)
                .Where(stream => stream.TimeZoneId.Equals(_timeZone));

            foreach (ScheduledStream stream in streamsNotInZone)
            {
                var (adjustDayOfWeek, localStartTime) = TimeZoneHelper.ConvertLocalTimeToDifferentTimeZone(
                    stream.LocalStartTime, stream.TimeZoneId, _timeZone);

                var (_, localEndTime) = TimeZoneHelper.ConvertLocalTimeToDifferentTimeZone(
                    stream.LocalEndTime, stream.TimeZoneId, _timeZone);

                stream.LocalStartTime = localStartTime;
                stream.LocalEndTime = localEndTime;
                stream.DayOfWeek += adjustDayOfWeek;
                stream.TimeZoneId = _timeZone;
            }

            return scheduleLookup;
        }
    }
}
