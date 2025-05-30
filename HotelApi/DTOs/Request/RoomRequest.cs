namespace HotelApi.DTOs.Request
{
    public class RoomRequest
    {
        public int Adultos { get; set; }
        public int Ninos { get; set; }

        public int? TipoHabitacionId { get; set; }
    }
}
