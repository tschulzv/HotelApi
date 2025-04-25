using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; }

        [Required]
        public DateTime Creacion { get; set; }

        [Required]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        public Solicitud Solicitud { get; set; }
    }
}
