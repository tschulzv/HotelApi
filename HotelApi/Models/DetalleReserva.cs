using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace HotelApi.Models
{
    public class DetalleReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // corresponde a id_detalle_reserva

        [Required]
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }

        public int? TipoHabitacionId { get; set; }
        public TipoHabitacion? TipoHabitacion { get; set; }

        public int? HabitacionId { get; set; } 
        public Habitacion Habitacion { get; set; }

        [Required]
        [Range(0, 99)]
        public int CantidadAdultos { get; set; }

        [Required]
        [Range(0, 99)]
        public int CantidadNinhos { get; set; }

        [Required]
        public int PensionId { get; set; }
        public Pension Pension { get; set; }

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
