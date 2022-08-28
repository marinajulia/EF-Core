using Microsoft.EntityFrameworkCore.Migrations;

namespace DominandoEFCore.Migrations
{
    public partial class TestePropagacaoDeDados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Departamentos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_meu_indice_composto",
                table: "Departamentos",
                columns: new[] { "Descricao", "Ativo" },
                unique: true,
                filter: "Descricao IS NOT NULL")
                .Annotation("SqlServer:FillFactor", 80);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropIndex(
                name: "idx_meu_indice_composto",
                table: "Departamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Departamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
