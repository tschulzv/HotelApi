using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class atributosUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cliente_TipoDocumentoId",
                table: "Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuario",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_Codigo",
                table: "Reserva",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_TipoDocumentoId_NumDocumento",
                table: "Cliente",
                columns: new[] { "TipoDocumentoId", "NumDocumento" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_Username",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_Codigo",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_TipoDocumentoId_NumDocumento",
                table: "Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_TipoDocumentoId",
                table: "Cliente",
                column: "TipoDocumentoId");
        }
    }
}
