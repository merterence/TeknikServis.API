using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknikServis.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServisTalebiKullanici : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adres",
                table: "ServisTalepleri");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ServisTalepleri");

            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "ServisTalepleri");

            migrationBuilder.AddColumn<int>(
                name: "KullaniciId",
                table: "ServisTalepleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServisTalepleri_KullaniciId",
                table: "ServisTalepleri",
                column: "KullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServisTalepleri_Kullanicilar_KullaniciId",
                table: "ServisTalepleri");

            migrationBuilder.DropIndex(
                name: "IX_ServisTalepleri_KullaniciId",
                table: "ServisTalepleri");

            migrationBuilder.DropColumn(
                name: "KullaniciId",
                table: "ServisTalepleri");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "ServisTalepleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ServisTalepleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "ServisTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
