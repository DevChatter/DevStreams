using System.Data;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192200)]
    public class AddTimeZoneIdToScheduledStream : Migration
    {
        public override void Up()
        {
            Alter.Table("ScheduledStreams")
                .AddColumn("TimeZoneId")
                .AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Column("TimeZoneId").FromTable("ScheduledStreams");
        }
    }
}