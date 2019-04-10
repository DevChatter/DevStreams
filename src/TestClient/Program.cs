using DevChatter.DevStreams.Client.GraphQL;
using GraphQL.Client;
using GraphQL.Common.Exceptions;
using System;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint = new Uri("http://localhost:57257/graphql");
            var graphQLClient = new GraphQLClient(endpoint);
            var channelGraphClient = new ChannelGraphClient(graphQLClient);

            var timeZone = "Europe/London";

            try
            {
                var result = channelGraphClient.GetChannelFutureStreams(1, timeZone, 0, 5).Result;

                Console.WriteLine($"{result.Name} (ID: {result.Id}");
                Console.WriteLine($"    URI = {result.Uri}");
                Console.WriteLine($"    Streamer's TZ = {result.TimeZoneId}");
                Console.WriteLine();

                Console.WriteLine($"Next 5 streams (in Time Zone {timeZone}):");
                foreach (var stream in result.FutureStreams)
                {
                    Console.WriteLine($"    Start Time: {stream.LocalStartTime}");
                    Console.WriteLine($"    End Time  : {stream.LocalEndTime}");
                    Console.WriteLine();
                }

                Console.WriteLine();

                Console.WriteLine($"Published Schedule (in Time Zone {timeZone}):");
                foreach (var slot in result.Schedule)
                {
                    Console.WriteLine($"    {slot.DayOfWeek} : {slot.LocalStartTime} to {slot.LocalEndTime}");
                }
                Console.WriteLine("Subjects channel is tagged with:");
                foreach (var tag in result.Tags)
                {
                    Console.WriteLine($"    {tag.Name}");
                }

                var result2 = channelGraphClient.GetChannels(timeZone).Result;

                Console.WriteLine($"Total # of channel in DevStreams = {result2.Count}");

                foreach (var channel in result2)
                {
                    Console.WriteLine($"  {channel.Name}");
                }
            }
            catch (GraphQLException gex)
            {
                Console.WriteLine(gex.Message);
            }

            Console.ReadLine();
        }
    }
}
