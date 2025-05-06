using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelApi.Models
{
    public class TipoHabitacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PrecioBase { get; set; }

        [Required]
        [Range(0, 1000)]
        public int CantidadDisponible {  get; set; }

        [Required]
        [Range(1, 6)]
        public int MaximaOcupacion { get; set; }
        [Required]
        [Range(1, 1000)]
        public int Tamanho { get; set; }

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

        // Propiedad de navegación inversa (colección)
        public ICollection<Habitacion> Habitaciones { get; set; }
        public ICollection<ImagenHabitacion> ImagenesHabitaciones { get; set; }
        public ICollection<Servicio> Servicios { get; set; }
    }

}
