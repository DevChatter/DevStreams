using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.DevStreams.Web.Data.Migrations
{
    public partial class AddStreamSessionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<int>(nullable: false),
                    ScheduledStreamId = table.Column<int>(nullable: false),
                    UtcStartTime = table.Column<long>(nullable: false),
                    UtcEndTime = table.Column<long>(nullable: false),
                    TzdbVersionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamSessions_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamSessions_ScheduledStream_ScheduledStreamId",
                        column: x => x.ScheduledStreamId,
                        principalTable: "ScheduledStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamSessions_ChannelId",
                table: "StreamSessions",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamSessions_ScheduledStreamId",
                table: "StreamSessions",
                column: "ScheduledStreamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamSessions");
        }
    }
}
