using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Pension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // id_pension

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioAdicional { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Creacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}
