using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Embyte.Migrations
{
    /// <inheritdoc />
    public partial class _28_12_23_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtractorEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeltaToPrevious = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractorEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteInfos",
                columns: table => new
                {
                    Url = table.Column<string>(type: "text", nullable: false),
                    HasData = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    SiteName = table.Column<string>(type: "text", nullable: false),
                    SiteType = table.Column<string>(type: "text", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Keywords = table.Column<string[]>(type: "text[]", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    FavIconUrl = table.Column<string>(type: "text", nullable: false),
                    ThemeColor = table.Column<string>(type: "text", nullable: false),
                    PersonName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteInfos", x => x.Url);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractorEntries");

            migrationBuilder.DropTable(
                name: "WebsiteInfos");
        }
    }
}
