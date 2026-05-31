using System;

namespace webGestionInventario.Model
{
    public class Venta
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public Venta()
        {
            Fecha = DateTime.Now;
        }
    }
}
