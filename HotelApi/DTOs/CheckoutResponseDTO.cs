namespace HotelApi.DTOs
{
    public class CheckoutResponseDTO
    {
        public int ReservaId { get; set; }
        public string CodigoReserva { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NochesEstadia { get; set; }
        public List<DetalleCostoHabitacionDTO> DetallesCostoHabitaciones { get; set; }
        public decimal SubtotalHabitaciones { get; set; }
        public decimal TotalAdicionalesPension { get; set; }
        public decimal Impuestos { get; set; }
        public decimal MontoTotalAPagar { get; set; }
    }

    public class DetalleCostoHabitacionDTO
    {
        public int DetalleReservaId { get; set; }
        public string TipoHabitacionNombre { get; set; }
        public int NumeroHabitacionAsignada { get; set; }
        public string PensionNombre { get; set; }
        public decimal SubtotalCostoHabitacion { get; set; } // (PrecioBaseHabitacion * Noches)
        public decimal SubtotalCostoPension { get; set; }   // (CostoPensionPorPersonaPorDia * Personas * Noches)
        public decimal TotalDetalle { get; set; }          // Suma de los dos subtotales anteriores
    }
}
