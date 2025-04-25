namespace HotelApi.Models
{
    public class ConsultaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Mensaje { get; set; }

        public bool Activo { get; set; }
    }
}
