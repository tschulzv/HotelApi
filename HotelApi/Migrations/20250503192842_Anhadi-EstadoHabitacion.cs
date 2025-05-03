using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class AnhadiEstadoHabitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Habitacion");

            migrationBuilder.AddColumn<int>(
                name: "EstadoHabitacionId",
                table: "Habitacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EstadoHabitacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoHabitacion", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habitacion_EstadoHabitacionId",
                table: "Habitacion",
                column: "EstadoHabitacionId");

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

            migrationBuilder.DropTable(
                name: "EstadoHabitacion");

            migrationBuilder.DropIndex(
                name: "IX_Habitacion_EstadoHabitacionId",
                table: "Habitacion");

            migrationBuilder.DropColumn(
                name: "EstadoHabitacionId",
                table: "Habitacion");

            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Habitacion",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
