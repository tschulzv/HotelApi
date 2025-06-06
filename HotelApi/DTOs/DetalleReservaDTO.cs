﻿namespace HotelApi.DTOs
{
    public class DetalleReservaDTO
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }

        public int? TipoHabitacionId { get; set; }

        public string? TipoHabitacion { get; set; }

        public int? HabitacionId { get; set; }

        public int? NumeroHabitacion { get; set; }

        public int CantidadAdultos { get; set; }

        public int CantidadNinhos { get; set; }

        public int PensionId { get; set; }

        public bool Activo { get; set; }
    }
}
