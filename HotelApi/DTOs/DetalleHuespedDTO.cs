namespace HotelApi.DTOs
{
    public class DetalleHuespedDTO
    {
        public int Id { get; set; }

        public int CheckInId { get; set; }

        public string Nombre { get; set; }

        public string NumDocumento { get; set; }

        public bool Activo { get; set; }
    }
}
