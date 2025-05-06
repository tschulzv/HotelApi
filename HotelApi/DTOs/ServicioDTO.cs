using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class ServicioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        // corresponde al nombre del icono en google material icons 
        public string IconName { get; set; }


    }
}
