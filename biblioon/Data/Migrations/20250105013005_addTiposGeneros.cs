using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioon.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTiposGeneros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Generos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Generos");
        }
    }
}
