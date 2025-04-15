using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // corresponde a id_reserva

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [StringLength(10)]
        public string Codigo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LlegadaEstimada { get; set; }

        [StringLength(256)]
        public string Comentarios { get; set; }

        [Required]
        public int Estado { get; set; } // puedes hacer un Enum para este campo

        [Required]
        [DataType(DataType.Date)]
        public DateTime Creacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        // Relación con detalles
        public ICollection<DetalleReserva> Detalles { get; set; }
    }
}
