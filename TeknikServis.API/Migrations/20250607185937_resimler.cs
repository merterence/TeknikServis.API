using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknikServis.API.Migrations
{
    /// <inheritdoc />
    public partial class resimler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TalepResimleri",
                table: "ServisTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TalepResimleri",
                table: "ServisTalepleri");
        }
    }
}
