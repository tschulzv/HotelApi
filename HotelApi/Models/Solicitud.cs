using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Solicitud
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ReservaId { get; set; }

        public Reserva? Reserva { get; set; }
        public int? CancelacionId { get; set; }

        public Cancelacion? Cancelacion { get; set; }
        public int? ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }

        public string? Tipo { get; set; } // "Reserva", "Cancelación", "Consulta" 

        [Required]
        public bool EsLeida { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        public string? Motivo { get; set; } // Motivo solo para solicitudes de cancelacion


    }
}
