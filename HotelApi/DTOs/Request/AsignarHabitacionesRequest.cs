namespace HotelApi.DTOs.Request
{
    public class AsignacionHabitacionDto
    {
        public int DetalleReservaId { get; set; }
        public int HabitacionId { get; set; }
    }

    public class AsignarHabitacionesRequest
    {
        public int ReservaId { get; set; }
        public List<AsignacionHabitacionDto> Asignaciones { get; set; }
    }
}
