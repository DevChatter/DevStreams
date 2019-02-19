using System.Data;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192052)]
    public class CreateStreamSessionsTable : Migration
    {
        public override void Up()
        {
            Create.Table("StreamSessions")
                .WithColumn("Id").AsInt32().PrimaryKey("PK_ScheduledStreams")
                .WithColumn("ChannelId").AsInt32().NotNullable()
                .WithColumn("ScheduledStreamId").AsInt32().NotNullable()
                .WithColumn("UtcStartTime").AsInt64().NotNullable()
                .WithColumn("UtcEndTime").AsInt64().NotNullable()
                .WithColumn("TzdbVersionId").AsString(255).NotNullable();

            Create.ForeignKey("FK_StreamSessions_Channels")
                .FromTable("StreamSessions").ForeignColumn("ChannelId")
                .ToTable("Channels").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_StreamSessions_ScheduledStreams")
                .FromTable("StreamSessions").ForeignColumn("ScheduledStreamId")
                .ToTable("ScheduledStreams").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("StreamSessions");
        }
    }
}