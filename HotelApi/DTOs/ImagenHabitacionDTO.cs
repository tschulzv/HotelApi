using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class ImagenHabitacionDTO
    {
        public int Id { get; set; }
        public int TipoHabitacionId { get; set; }
        public byte[] Imagen { get; set; }

    }
}
