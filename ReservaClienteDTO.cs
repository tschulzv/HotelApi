using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs
{
    public class ReservaClienteDTO
    {
        [Required]
        public ClienteDTO InformacionCliente { get; set; }
        [Required]
        public ReservaDTO InformacionReserva { get; set; }
    }
}
