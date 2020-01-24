using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Schoolman.Student.Infrastructure.Migrations
{
    public partial class Refreshtokenchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshTokens");
            

            migrationBuilder.AddColumn<long>(
                name: "Created_unixTime",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Expires_unixTime",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_unixTime",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Expires_unixTime",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Expires",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
