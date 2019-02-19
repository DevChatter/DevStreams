using System.Data;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192044)]
    public class CreateScheduledStreamsTable : Migration
    {
        public override void Up()
        {
            Create.Table("ScheduledStreams")
                .WithColumn("Id").AsInt32().PrimaryKey("PK_ScheduledStreams")
                .WithColumn("ChannelId").AsInt32()
                .WithColumn("DayOfWeek").AsString(255)
                .WithColumn("LocalStartTime").AsInt64()
                .WithColumn("LocalEndTime").AsInt64();

            Create.ForeignKey("FK_ScheduledStreams_Channels")
                .FromTable("ScheduledStreams").ForeignColumn("ChannelId")
                .ToTable("Channels").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("ScheduledStreams");
        }
    }
}