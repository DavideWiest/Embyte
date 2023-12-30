using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embyte.Migrations
{
    /// <inheritdoc />
    public partial class _28_12_23_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "WebsiteInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "DataChanged",
                table: "ExtractorEntries",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "WebsiteInfos");

            migrationBuilder.DropColumn(
                name: "DataChanged",
                table: "ExtractorEntries");
        }
    }
}
