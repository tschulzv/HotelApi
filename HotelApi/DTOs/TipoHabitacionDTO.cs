using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class TipoHabitacionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int CantidadDisponible { get; set; }
        public int MaximaOcupacion { get; set; }
        public int Tamanho { get; set; }
        public List<ServicioDTO> Servicios { get; set; } = new List<ServicioDTO>();
        public List<ImagenHabitacionDTO> Imagenes { get; set; } = new List<ImagenHabitacionDTO>();
    }
}
