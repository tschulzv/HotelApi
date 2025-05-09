namespace HotelApi.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string NumDocumento { get; set; }

        public string? Ruc { get; set; }

        public int TipoDocumentoId { get; set; }

        public string Nacionalidad { get; set; }

        public string Comentarios { get; set; }

        public bool Activo { get; set; }

        public DateTime Creacion {  get; set; }
    }
}
