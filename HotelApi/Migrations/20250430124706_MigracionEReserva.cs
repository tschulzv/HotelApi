using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class MigracionEReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "EstadoReserva",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Actualizacion",
                table: "EstadoReserva",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Creacion",
                table: "EstadoReserva",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "EstadoReserva");

            migrationBuilder.DropColumn(
                name: "Actualizacion",
                table: "EstadoReserva");

            migrationBuilder.DropColumn(
                name: "Creacion",
                table: "EstadoReserva");
        }
    }
}
