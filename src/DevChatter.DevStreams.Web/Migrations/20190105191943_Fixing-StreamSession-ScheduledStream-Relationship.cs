using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.DevStreams.Web.Migrations
{
    public partial class FixingStreamSessionScheduledStreamRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamSessions_ScheduledStream_ScheduledStreamId",
                table: "StreamSessions");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledStreamId",
                table: "StreamSessions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamSessions_ScheduledStream_ScheduledStreamId",
                table: "StreamSessions",
                column: "ScheduledStreamId",
                principalTable: "ScheduledStream",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamSessions_ScheduledStream_ScheduledStreamId",
                table: "StreamSessions");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduledStreamId",
                table: "StreamSessions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StreamSessions_ScheduledStream_ScheduledStreamId",
                table: "StreamSessions",
                column: "ScheduledStreamId",
                principalTable: "ScheduledStream",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
