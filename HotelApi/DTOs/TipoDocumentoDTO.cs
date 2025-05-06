using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class TipoDocumentoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }
}
