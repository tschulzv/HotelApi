using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.Models
{
    public class TipoDocumento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ids autoincrementados
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime Creacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        // Propiedad de navegación inversa (colección)
        public ICollection<Cliente> Clientes { get; set; }
    }
}
