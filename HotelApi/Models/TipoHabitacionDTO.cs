using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class TipoHabitacionDTO
    {
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
        public int CantidadDisponible { get; set; }

        public ICollection<ServicioDTO> Servicios { get; set; }
    }
}
