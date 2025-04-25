using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class SolicitudDTO
    {
        public int Id { get; set; }

        public int? ReservaId { get; set; }

        public int? CancelacionId { get; set; }

        public int? ConsultaId { get; set; }

        [Required]
        public bool EsLeida { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; } // dejamos la fecha de creacion porque nos servira para ordenar las solicitudes por fecha de llegada

    }
}
