using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.DevStreams.Web.Data.Migrations
{
    public partial class AddCountryCodeToChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Channels",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Channels");
        }
    }
}
