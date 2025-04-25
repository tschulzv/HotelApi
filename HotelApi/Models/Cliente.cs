using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelApi.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ids autoincrementados
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Formato de correo electrónico inválido")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 9)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NumDocumento { get; set; }

      
        [Required]
        // FK y propiedad de navegacion del tipo de documento 
        public int TipoDocumentoId { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        [Required]
        [StringLength(50)]
        public string Nacionalidad { get; set; }

        [StringLength(256)]
        public string Comentarios { get; set; }

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

        // relación uno a muchos (un cliente puede tener muchas reservas)
        //public List<Rent> Rents { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }
}
