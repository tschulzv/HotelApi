namespace HotelApi.DTOs
{
    public class CancelacionDTO
    {
        public int Id { get; set; }

        public List<int>? DetalleReservaIds { get; set; }

        public int? ReservaId { get; set; }

        public ReservaDTO? Reserva { get; set; }

        public string? Motivo { get; set; }

        public bool Activo { get; set; }
    }
}
