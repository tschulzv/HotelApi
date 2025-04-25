namespace HotelApi.Models
{
    public class CheckinDTO
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public bool Activo { get; set; }

        public List<DetalleHuespedDTO> DetalleHuespedes { get; set; }
    }
}
