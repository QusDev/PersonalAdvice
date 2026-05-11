using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBpmAddAlbumNameInTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BPM",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "AlbumName",
                table: "Tracks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "MediaContents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlbumName",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "MediaContents");

            migrationBuilder.AddColumn<int>(
                name: "BPM",
                table: "Tracks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
