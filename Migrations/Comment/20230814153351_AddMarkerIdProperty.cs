using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WGO_API.Migrations.Comment
{
    /// <inheritdoc />
    public partial class AddMarkerIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarkerId",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkerId",
                table: "Comments");
        }
    }
}
