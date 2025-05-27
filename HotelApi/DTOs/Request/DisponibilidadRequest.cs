namespace HotelApi.DTOs.Request
{
    public class DisponibilidadRequest
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<RoomRequest> HabitacionesSolicitadas { get; set; } = new();
        // Campo opcional
        public int? TipoHabitacionId { get; set; }

    }

}
