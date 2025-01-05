using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioon.Data.Migrations
{
    /// <inheritdoc />
    public partial class addShNameGeneros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShName",
                table: "Generos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShName",
                table: "Generos");
        }
    }
}
