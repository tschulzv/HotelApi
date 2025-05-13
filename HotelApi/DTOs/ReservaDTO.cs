using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class ReservaDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public string? NombreCliente { get; set; }
        public string Codigo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        public TimeOnly? LlegadaEstimada { get; set; }
        public string Comentarios { get; set; }

        public int EstadoId { get; set; }

        public ICollection<DetalleReservaDTO>? Detalles { get; set; }


    }
}
