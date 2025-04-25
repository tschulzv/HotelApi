using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Solicitud
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TipoSolicitudId { get; set; }

        public int? ReservaId { get; set; }

        public Reserva? Reserva { get; set; }
        public int? CancelacionId { get; set; }

        public Cancelacion? Cancelacion { get; set; }
        public int? ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }

        [Required]
        public bool EsLeida { get; set; }

        [Required]
        public DateTime Creacion { get; set; }

        [Required]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;


    }
}
