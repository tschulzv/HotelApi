using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class ImagenHabitacionDTO
    {
        public int Id { get; set; }

        [Required]
        public int TipoHabitacionId { get; set; }

        [Required]
        public byte[] Imagen { get; set; }

    }
}
