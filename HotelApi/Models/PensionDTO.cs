using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class PensionDTO
    {
        public int Id { get; set; }  // id_pension

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PrecioAdicional { get; set; }

    }
}
