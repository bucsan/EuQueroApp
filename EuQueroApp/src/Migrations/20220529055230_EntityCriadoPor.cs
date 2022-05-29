using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuQueroApp.Migrations
{
    public partial class EntityCriadoPor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Produtos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CriadoPor",
                table: "Categorias",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CriadoPor",
                table: "Categorias");
        }
    }
}
