using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class addtipohabitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoHabitacionId",
                table: "DetalleReserva",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReserva_TipoHabitacionId",
                table: "DetalleReserva",
                column: "TipoHabitacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleReserva_TipoHabitacion_TipoHabitacionId",
                table: "DetalleReserva",
                column: "TipoHabitacionId",
                principalTable: "TipoHabitacion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleReserva_TipoHabitacion_TipoHabitacionId",
                table: "DetalleReserva");

            migrationBuilder.DropIndex(
                name: "IX_DetalleReserva_TipoHabitacionId",
                table: "DetalleReserva");

            migrationBuilder.DropColumn(
                name: "TipoHabitacionId",
                table: "DetalleReserva");
        }
    }
}
