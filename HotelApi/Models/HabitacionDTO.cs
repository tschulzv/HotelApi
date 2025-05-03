namespace HotelApi.Models
{
    public class HabitacionDTO
    {
        public int Id { get; set; }
        public int TipoHabitacionId { get; set; }
        public int NumeroHabitacion { get; set; }
        public int EstadoHabitacionId { get; set; }
        public bool Activo { get; set; }
    }
}
