﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Checkin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public Reserva Reserva { get; set; }

        [Required]
        public DateTime FechaCheckIn { get; set; }

        [Required]
        public DateTime Creacion { get; set; }

        [Required]
        public DateTime Actualizacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;


    }
}
