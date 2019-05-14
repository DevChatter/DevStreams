using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201904182000)]
    public class AddChannelImageToTwitchChannel : Migration
    {
        private const string ColumnName = "ImageUrl";
        private const string TableName = "TwitchChannels";
        private const int MaxUrlLength = 2000;

        public override void Up()
        {
            Alter.Table(TableName)
                .AddColumn(ColumnName)
                .AsString(MaxUrlLength)
                .WithDefaultValue("")
                .NotNullable();
        }


        public override void Down()
        {
            Delete.Column(ColumnName)
                .FromTable(TableName);
        }

    }
}
