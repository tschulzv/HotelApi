namespace HotelApi.DTOs
{
    public class CheckoutDTO
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public bool Activo { get; set; }
    }
}
