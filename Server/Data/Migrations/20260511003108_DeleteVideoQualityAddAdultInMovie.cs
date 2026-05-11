using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteVideoQualityAddAdultInMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoQuality",
                table: "Movies");

            migrationBuilder.AddColumn<bool>(
                name: "Adult",
                table: "Movies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adult",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "VideoQuality",
                table: "Movies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
