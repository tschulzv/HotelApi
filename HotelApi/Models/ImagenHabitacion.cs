using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class ImagenHabitacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TipoHabitacionId { get; set; }

        public TipoHabitacion TipoHabitacion { get; set; }

        [Required]
        [MaxLength(255)]
        public string Url { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}
