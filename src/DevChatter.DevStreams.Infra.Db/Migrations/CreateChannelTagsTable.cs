using System.Data;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192028)]
    public class CreateChannelTagsTable : Migration
    {
        public override void Up()
        {
            Create.Table("ChannelTags")
                .WithColumn("ChannelId").AsInt32()
                .WithColumn("TagId").AsInt32();

            Create.PrimaryKey("PK_ChannelTags")
                .OnTable("ChannelTags")
                .Columns("ChannelId", "TagId");

            Create.ForeignKey("FK_ChannelTags_Channels")
                .FromTable("ChannelTags").ForeignColumn("ChannelId")
                .ToTable("Channels").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_ChannelTags_Tags")
                .FromTable("ChannelTags").ForeignColumn("TagId")
                .ToTable("Tags").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("ChannelTags");
        }
    }
}