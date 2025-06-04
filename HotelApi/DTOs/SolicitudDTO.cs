using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class SolicitudDTO
    {
        public int Id { get; set; }

        public int? ReservaId { get; set; }

        public ReservaDTO? Reserva{ get; set; }

        public int? CancelacionId { get; set; }

        public CancelacionDTO? Cancelacion { get; set; }

        public int? ConsultaId { get; set; }

        public ConsultaDTO? Consulta{ get; set; }
        public string? Tipo { get; set; } 

        [Required]
        public bool EsLeida { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]

        public DateTime Creacion { get; set; } // dejamos la fecha de creacion porque nos servira para ordenar las solicitudes por fecha de llegada

        public string? Motivo { get; set; } // El motivo es solo para la solictud de cancelacion

    }
}
