using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEdiLivroBarCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "EdiLivros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "EdiLivros");
        }
    }
}
