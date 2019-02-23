using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192003)]
    public class CreateChannelsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Channels")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255)
                .WithColumn("Uri").AsString(255).NotNullable()
                .WithColumn("CountryCode").AsString(3).NotNullable()
                .WithColumn("TimeZoneId").AsString(255).NotNullable();

            Create.Index("ix_Channels_Name")
                .OnTable("Channels")
                .OnColumn("Name")
                .Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Table("Channels");
        }
    }
}
