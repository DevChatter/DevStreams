using System.Data;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902261944)]
    public class CreateChannelPermissionsTable : Migration
    {
        private const string TABLE_NAME = "ChannelPermissions";

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                .WithColumn("ChannelId").AsInt32().NotNullable()
                .WithColumn("UserId").AsString(450).NotNullable()
                .WithColumn("ChannelRole").AsString(256).NotNullable();

            Create.PrimaryKey($"PK_{TABLE_NAME}")
                .OnTable(TABLE_NAME)
                .Columns("ChannelId", "UserId");

            Create.Index($"ix_{TABLE_NAME}_ChannelId")
                .OnTable(TABLE_NAME)
                .OnColumn("ChannelId")
                .Ascending()
                .WithOptions().NonClustered();

            Create.Index($"ix_{TABLE_NAME}_UserId")
                .OnTable(TABLE_NAME)
                .OnColumn("UserId")
                .Ascending()
                .WithOptions().NonClustered();

            Create.ForeignKey($"FK_{TABLE_NAME}_Channels")
                .FromTable(TABLE_NAME).ForeignColumn("ChannelId")
                .ToTable("Channels").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey($"FK_{TABLE_NAME}_Users")
                .FromTable(TABLE_NAME).ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}