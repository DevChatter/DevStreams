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

		public override void Up()
		{
			Alter.Table(TableName)
				.AddColumn(ColumnName)
				.AsString(4000) // Note: 4000 is Db max value in Fluent Nhibernate. Not sure if this is a good idea or not? -- DAR 18/04/2019
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
