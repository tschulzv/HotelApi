using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class ReservaDTO
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(10)]
        public string Codigo { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        public TimeOnly? LlegadaEstimada { get; set; }

        [StringLength(256)]
        public string Comentarios { get; set; }

        [Required]
        public int EstadoId { get; set; }

        public ICollection<DetalleReservaDTO>? Detalles { get; set; }


    }
}
