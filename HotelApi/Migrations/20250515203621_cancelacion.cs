using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class cancelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancelacion_DetalleReserva_DetalleReservaId",
                table: "Cancelacion");

            migrationBuilder.AlterColumn<int>(
                name: "DetalleReservaId",
                table: "Cancelacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "Cancelacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cancelacion_ReservaId",
                table: "Cancelacion",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancelacion_DetalleReserva_DetalleReservaId",
                table: "Cancelacion",
                column: "DetalleReservaId",
                principalTable: "DetalleReserva",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancelacion_Reserva_ReservaId",
                table: "Cancelacion",
                column: "ReservaId",
                principalTable: "Reserva",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancelacion_DetalleReserva_DetalleReservaId",
                table: "Cancelacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Cancelacion_Reserva_ReservaId",
                table: "Cancelacion");

            migrationBuilder.DropIndex(
                name: "IX_Cancelacion_ReservaId",
                table: "Cancelacion");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "Cancelacion");

            migrationBuilder.AlterColumn<int>(
                name: "DetalleReservaId",
                table: "Cancelacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cancelacion_DetalleReserva_DetalleReservaId",
                table: "Cancelacion",
                column: "DetalleReservaId",
                principalTable: "DetalleReserva",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
