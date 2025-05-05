using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknikServis.API.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Kullanici_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri");

            migrationBuilder.AddForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri");

            migrationBuilder.AddForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
