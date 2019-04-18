using FluentMigrator;
using System;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201904182027)]
    public class AddDescriptionToTwitchChannelsTable : Migration
    {
        public override void Up()
        {
            Alter.Table("TwitchChannels")
                .AddColumn("Description")
                .AsString(Int32.MaxValue)
                .WithDefaultValue("")
                .NotNullable();

            Alter.Column("Description")
                .OnTable("TwitchChannels")
                .AsString(Int32.MaxValue)
                .NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Description").FromTable("TwitchChannels");
        }
    }
}