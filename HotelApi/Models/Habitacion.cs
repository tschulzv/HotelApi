using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace HotelApi.Models
{
    public class Habitacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TipoHabitacionId{ get; set; }
        public TipoHabitacion TipoHabitacion { get; set; }

        [Required]
        [Range(100,1000)]
        public int NumeroHabitacion { get; set; }

        //cambie en vez de bool Disponible
        [Required]
        public int EstadoHabitacionId { get; set; }
        public EstadoHabitacion EstadoHabitacion { get; set; }

        // falta lo de tamaño y capacidad

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        public Collection<DetalleReserva> DetalleReservas { get; set; }
    }
}
