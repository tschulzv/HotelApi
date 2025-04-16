using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Checkout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [Required]
        public DateTime FechaCheckOut { get; set; }

        [Required]
        public DateTime Creacion { get; set; }

        [Required]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}
