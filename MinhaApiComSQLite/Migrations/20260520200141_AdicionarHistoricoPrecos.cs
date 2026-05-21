using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaApiComSQLite.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarHistoricoPrecos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoPreco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoAnterior = table.Column<double>(type: "REAL", nullable: false),
                    PrecoNovo = table.Column<double>(type: "REAL", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPreco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPreco_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPreco_ProdutoId",
                table: "HistoricoPreco",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoPreco");
        }
    }
}
