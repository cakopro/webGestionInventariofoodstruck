namespace webGestionInventario.Model
{
    public class VentaHistorialDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalVenta { get; set; }
    }
}