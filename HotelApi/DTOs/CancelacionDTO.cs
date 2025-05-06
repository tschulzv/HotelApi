namespace HotelApi.DTOs
{
    public class CancelacionDTO
    {
        public int Id { get; set; }

        public int DetalleReservaId { get; set; }

        public string Motivo { get; set; }

        public bool Activo { get; set; }
    }
}
