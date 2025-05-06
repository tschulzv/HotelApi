using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class denuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacion_EstadoHabitacion_EstadoHabitacionId",
                table: "Habitacion");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoHabitacionId",
                table: "Habitacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacion_EstadoHabitacion_EstadoHabitacionId",
                table: "Habitacion",
                column: "EstadoHabitacionId",
                principalTable: "EstadoHabitacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacion_EstadoHabitacion_EstadoHabitacionId",
                table: "Habitacion");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoHabitacionId",
                table: "Habitacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacion_EstadoHabitacion_EstadoHabitacionId",
                table: "Habitacion",
                column: "EstadoHabitacionId",
                principalTable: "EstadoHabitacion",
                principalColumn: "Id");
        }
    }
}
