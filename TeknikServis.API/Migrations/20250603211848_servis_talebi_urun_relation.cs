using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknikServis.API.Migrations
{
    /// <inheritdoc />
    public partial class servis_talebi_urun_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UrunId",
                table: "ServisTalepleri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServisTalepleri_UrunId",
                table: "ServisTalepleri",
                column: "UrunId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServisTalepleri_Urunler_UrunId",
                table: "ServisTalepleri",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServisTalepleri_Urunler_UrunId",
                table: "ServisTalepleri");

            migrationBuilder.DropIndex(
                name: "IX_ServisTalepleri_UrunId",
                table: "ServisTalepleri");

            migrationBuilder.DropColumn(
                name: "UrunId",
                table: "ServisTalepleri");
        }
    }
}
