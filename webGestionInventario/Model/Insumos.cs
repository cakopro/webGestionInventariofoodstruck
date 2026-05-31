using System;

namespace webGestionInventario.Model
{
    public class Insumos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int StockActual { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public int Id_Proveedor { get; set; }
        public bool Estado { get; set; }
        public string NombreProveedor { get; set; }

        public Insumos()
        {
        }
    }
}
