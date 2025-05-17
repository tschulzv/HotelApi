using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Cancelacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? DetalleReservaId { get; set; }
        public DetalleReserva? DetalleReserva { get; set; }

        public int? ReservaId { get; set; }

        public Reserva? Reserva { get; set; }

        [Required]
        [StringLength(200)]
        public string Motivo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}
