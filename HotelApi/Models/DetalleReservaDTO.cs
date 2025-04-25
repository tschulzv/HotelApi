namespace HotelApi.Models
{
    public class DetalleReservaDTO
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public int? HabitacionId { get; set; }

        public int CantidadAdultos { get; set; }

        public int CantidadNinhos { get; set; }

        public int PensionId { get; set; }

        public bool Activo { get; set; }
    }
}
