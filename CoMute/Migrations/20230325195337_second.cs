using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoMute.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateLeft",
                table: "Opportunities");

            migrationBuilder.AddColumn<bool>(
                name: "Joined",
                table: "Opportunities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Joined",
                table: "Opportunities");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLeft",
                table: "Opportunities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
