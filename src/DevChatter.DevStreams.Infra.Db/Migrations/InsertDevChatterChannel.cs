using System.Collections.Generic;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902230059)]
    public class InsertDevChatterChannel : Migration
    {
        public override void Up()
        {
            var channel = new
            {
                Name = "DevChatter",
                Uri = "https://www.twitch.tv/devchatter",
                CountryCode = "US",
                TimeZoneId = "America/New_York"
            };
            Insert.IntoTable("Channels").Row(channel);
        }

        public override void Down()
        {
            Delete.FromTable("Channels").Row(new { Name = "DevChatter" });
        }
    }
}