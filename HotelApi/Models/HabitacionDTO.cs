namespace HotelApi.Models
{
    public class HabitacionDTO
    {
        public int Id { get; set; }
        public int TipoHabitacionId { get; set; }
        public int NumeroHabitacion { get; set; }
        public bool Disponible { get; set; }
        public bool Activo { get; set; }
    }
}
