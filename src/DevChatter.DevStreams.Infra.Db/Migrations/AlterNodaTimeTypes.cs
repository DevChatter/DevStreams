using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902391908)]
    public class AlterNodaTimeTypes : Migration
    {
        public override void Up()
        {
            Delete.FromTable("StreamSessions").AllRows();

            Alter.Table("StreamSessions").AlterColumn("UtcStartTime").AsDateTime().NotNullable();
            Alter.Table("StreamSessions").AlterColumn("UtcEndTime").AsDateTime().NotNullable();

            Delete.FromTable("ScheduledStreams").AllRows();

            Alter.Table("ScheduledStreams").AlterColumn("LocalStartTime").AsDateTime().NotNullable();
            Alter.Table("ScheduledStreams").AlterColumn("LocalEndTime").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.FromTable("StreamSessions").AllRows();

            Alter.Table("StreamSessions").AlterColumn("UtcStartTime").AsInt64().NotNullable();
            Alter.Table("StreamSessions").AlterColumn("UtcEndTime").AsInt64().NotNullable();

            Delete.FromTable("ScheduledStreams").AllRows();

            Alter.Table("ScheduledStreams").AlterColumn("LocalStartTime").AsInt64().NotNullable();
            Alter.Table("ScheduledStreams").AlterColumn("LocalEndTime").AsInt64().NotNullable();
        }
    }
}