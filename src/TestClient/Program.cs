using DevChatter.DevStreams.Client.GraphQL;
using GraphQL.Client;
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

            var result = channelGraphClient.GetChannel(1, timeZone).Result;

            Console.WriteLine($"{result.Name} (ID: {result.Id}");
            Console.WriteLine($"    URI = {result.Uri}");
            Console.WriteLine($"    Streamer's TZ = {result.TimeZoneId}");
            Console.WriteLine();
            Console.WriteLine($"Next Stream (in Time Zone {timeZone}):");
            Console.WriteLine($"    Start Time: {result.NextStream.LocalStartTime}");
            Console.WriteLine($"    End Time  : {result.NextStream.LocalEndTime}");

            Console.ReadLine();
        }
    }
}
