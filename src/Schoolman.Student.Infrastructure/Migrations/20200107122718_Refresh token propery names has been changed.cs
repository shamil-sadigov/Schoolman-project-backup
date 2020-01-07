using Microsoft.EntityFrameworkCore.Migrations;

namespace Schoolman.Student.Infrastructure.Migrations
{
    public partial class Refreshtokenproperynameshasbeenchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expires_unixTime",
                table: "RefreshTokens",
                newName: "Expiration_time");

            migrationBuilder.RenameColumn(
                name: "Created_unixTime",
                table: "RefreshTokens",
                newName: "Creation_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration_time",
                table: "RefreshTokens",
                newName: "Expires_unixTime");

            migrationBuilder.RenameColumn(
                name: "Creation_time",
                table: "RefreshTokens",
                newName: "Created_unixTime");
        }
    }
}
