using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknikServis.API.Migrations
{
    /// <inheritdoc />
    public partial class AddServisTalebiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Urun",
                table: "ServisTalepleri",
                newName: "UrunAdi");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "ServisTalepleri",
                newName: "TalepTarihi");

            migrationBuilder.AlterColumn<bool>(
                name: "Durum",
                table: "ServisTalepleri",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrunAdi",
                table: "ServisTalepleri",
                newName: "Urun");

            migrationBuilder.RenameColumn(
                name: "TalepTarihi",
                table: "ServisTalepleri",
                newName: "Tarih");

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "ServisTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
