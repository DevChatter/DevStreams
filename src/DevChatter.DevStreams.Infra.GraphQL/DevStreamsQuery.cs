using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Twitch;
using DevChatter.DevStreams.Infra.GraphQL.Types;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.GraphQL
{
    public class DevStreamsQuery : ObjectGraphType
    {
        private readonly IChannelRepository _channelRepo;
        private readonly ITwitchStreamService _twitchService;

        public DevStreamsQuery(IChannelRepository channelRepo, ITwitchStreamService twitchService,
            IChannelSearchService channelSearchService, ITagService tagService)
        {
            _channelRepo = channelRepo;
            _twitchService = twitchService;

            Field<ListGraphType<ChannelType>>("channels",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<IdGraphType>>
                    {
                        Name = "tagIds",
                        DefaultValue = new List<int>()
                    }
                ),
                resolve: ctx =>
                {
                    List<int> tagIds = ctx.GetArgument<List<int>>("tagIds");
                    if (tagIds.Any())
                    {
                        return channelRepo.GetChannelsByTagIds(tagIds.ToArray());
                    }
                    return channelRepo.GetAll<Channel>();
                });

            Field<ListGraphType<ChannelType>>("channelsHavingTags",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>>
                    {
                        Name = "tagIds",
                        DefaultValue = new List<int>()
                    }
                ),
                resolve: ctx =>
                {
                    List<int> tagIds = ctx.GetArgument<List<int>>("tagIds");
                    return channelSearchService.GetChannelsByTagMatches(tagIds.ToArray());
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

            Field<ChannelType>("channelSoundex",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "name"
                }),
                resolve: ctx =>
                {
                    var name = ctx.GetArgument<string>("name");
                    return channelSearchService.GetChannelSoundex(name);
                });
            Field<ListGraphType<ChannelType>>("liveChannels",
                resolve: ctx =>
                {
                    return GetLiveChannels();
                });
            Field<ListGraphType<TagType>>("tagsInUse",
                resolve: ctx =>
                {
                    return tagService.GetTagsInUse();
                });
        }

        private async Task<List<Channel>> GetLiveChannels()
        {
            var channels = await _channelRepo.GetAll<Channel>();
            var liveChannelIds = await GetLiveTwitchChannels();

            return channels
                .Where( c=> liveChannelIds.Contains(c.Id))
                .ToList();
        }

        private async Task<List<int>> GetLiveTwitchChannels()
        {
            var twitchChannels = await _channelRepo.GetAll<TwitchChannel>();

            var twitchIds = twitchChannels.Select(t => t.TwitchId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var liveChannelTwitchIds = (await _twitchService.GetChannelLiveStates(twitchIds))
                .Where(x => x.IsLive)
                .Select(x => x.TwitchId)
                .ToList();

            return twitchChannels
                .Where(c => liveChannelTwitchIds.Contains(c.TwitchId))
                .Select(c => c.ChannelId)
                .ToList();
        }
    }
}
