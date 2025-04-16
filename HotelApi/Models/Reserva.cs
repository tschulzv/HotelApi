using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [StringLength(10)]
        public string Codigo { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        public DateTime? LlegadaEstimada { get; set; }

        [StringLength(256)]
        public string Comentarios { get; set; }

        [Required]
        public int EstadoId { get; set; }
        public EstadoReserva Estado { get; set; }

        [Required]
        public DateTime Creacion { get; set; }

        [Required]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        public ICollection<DetalleReserva> Detalles { get; set; }

        public Checkin Checkin { get; set; }
    }
}
