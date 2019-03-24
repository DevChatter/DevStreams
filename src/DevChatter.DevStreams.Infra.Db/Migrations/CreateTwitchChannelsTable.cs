using FluentMigrator;
using System.Data;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201903192004)]
    public class CreateTwitchChannelsTable : Migration
    {
        public override void Up()
        {
            Create.Table("TwitchChannels")
                .WithColumn("ChannelId").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("TwitchId").AsString(255).NotNullable()
                .WithColumn("TwitchName").AsString(255).NotNullable()
                .WithColumn("IsAffiliate").AsBoolean().NotNullable()
                .WithColumn("IsPartner").AsBoolean().NotNullable();

            Create.ForeignKey("FK_TwitchChannels_Channels")
                .FromTable("TwitchChannels").ForeignColumn("ChannelId")
                .ToTable("Channels").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("TwitchChannels");
        }
    }
}
