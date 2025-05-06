namespace HotelApi.DTOs
{
    public class HabitacionDTO
    {
        public int Id { get; set; }
        public int NumeroHabitacion { get; set; }
        public int TipoHabitacionId { get; set; }
        public string TipoHabitacionNombre { get; set; } = string.Empty;
        public int EstadoHabitacionId { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
    }
}
