using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages
{
    public class InitDataModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IScheduledStreamService _scheduledStreamService;
        private readonly Random _random = new Random();
        private readonly List<Channel> _channels = new List<Channel>();

        public InitDataModel(ApplicationDbContext db, 
            IScheduledStreamService scheduledStreamService)
        {
            _db = db;
            _scheduledStreamService = scheduledStreamService;
        }

        public void OnGet()
        {
            TryCreateTags();
            TryCreateChannels();
            TryAddTags();
            TryAddScheduledStreams();
        }

        private void TryAddScheduledStreams()
        {
            foreach (Channel channel in _channels)
            {
                for (int i = 0; i < 2; i++)
                {
                    LocalTime localStartTime = new LocalTime(9, 0).PlusHours(_random.Next(0,10));
                    var scheduledStream = new ScheduledStream
                    {
                        DayOfWeek = (IsoDayOfWeek)_random.Next(1,8),
                        LocalStartTime = localStartTime,
                        LocalEndTime = localStartTime.PlusHours(_random.Next(1,6)),
                    };
                    _scheduledStreamService.AddScheduledStreamToChannel(
                        channel, scheduledStream);
                }
            }

            _db.SaveChanges();
        }

        private void TryAddTags()
        {
            if (_channels.Any())
            {
                List<Tag> tags = _db.Tags.ToList();
                foreach (var channel in _channels)
                {
                    foreach (Tag tag in PickRandom(tags, _random.Next(1, 4)))
                    {
                        var channelTag = new ChannelTag
                        {
                            Channel = channel,
                            Tag = tag
                        };
                        channel.Tags.Add(channelTag);
                    }
                }

                _db.SaveChanges();
            }
        }

        private void TryCreateTags()
        {
            if (!_db.Tags.Any())
            {
                _db.Tags.Add(new Tag{ Name = "C#", Description = "C#"});
                _db.Tags.Add(new Tag{ Name = "JavaScript", Description = "JavaScript"});
                _db.Tags.Add(new Tag{ Name = "Java", Description = "Java"});
                _db.Tags.Add(new Tag{ Name = "Python", Description = "Python"});
                _db.Tags.Add(new Tag{ Name = "Windows", Description = "Windows"});
                _db.Tags.Add(new Tag{ Name = "Linux", Description = "Linux"});
                _db.Tags.Add(new Tag{ Name = "English", Description = "English"});
                _db.Tags.Add(new Tag{ Name = "German", Description = "German"});
                _db.Tags.Add(new Tag{ Name = "French", Description = "French"});
                _db.Tags.Add(new Tag{ Name = "GameDev", Description = "GameDev"});
                _db.Tags.Add(new Tag{ Name = "WebDev", Description = "WebDev"});
                _db.SaveChanges();
            }
        }

        private void TryCreateChannels()
        {
            if (!_db.Channels.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var channel = new Channel
                    {
                        TimeZoneId = "America/New_York",
                        CountryCode = "US",
                        Name = $"DevChatter{i}",
                        Uri = $"https://www.Twitch.tv/DevChatter{i}"
                    };
                    _channels.Add(channel);
                }
                _db.Channels.AddRange(_channels);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Pull random items from set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="count">number of unique items to pull</param>
        /// <returns></returns>
        private List<T> PickRandom<T>(List<T> items, int count)
        {
            var result = new List<T>();
            int index = _random.Next(0, items.Count);
            for (int i = 0; i < count; i++)
            {
                int moddedIndex = (index + i) % items.Count;
                result.Add(items[moddedIndex]);
            }
            return result;
        }
    }
}