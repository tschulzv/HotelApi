using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class PensionDTO
    {
        public int Id { get; set; }  // id_pension
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal PrecioAdicional { get; set; }

    }
}
