namespace webGestionInventario.Model
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Estado { get; set; }

        public Producto()
        {
            Estado = true;
        }
    }
}
