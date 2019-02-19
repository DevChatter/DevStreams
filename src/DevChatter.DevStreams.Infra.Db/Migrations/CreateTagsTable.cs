using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902192023)]
    public class CreateTagsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Tags")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Description").AsString(255).NotNullable();

            Create.Index("ix_Tags_Name")
                .OnTable("Tags")
                .OnColumn("Name")
                .Ascending()
                .WithOptions().Unique()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Table("Tags");
        }
    }
}