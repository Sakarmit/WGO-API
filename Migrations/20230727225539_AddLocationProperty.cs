using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WGO_API.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "Markers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "Markers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Markers");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Markers");
        }
    }
}
