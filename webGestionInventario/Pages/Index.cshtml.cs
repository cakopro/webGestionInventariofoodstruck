using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public IndexModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public decimal GananciaHoy { get; set; }

        public IList<Insumos> ListaInsumosCaducados { get; set; } = new List<Insumos>();
        public IList<Insumos> ListaInsumosBajoStock { get; set; } = new List<Insumos>();


        public async Task OnGetAsync()
        {
            GananciaHoy = await _databaseHelper.ObtenerGananciaDelDia();
            ListaInsumosCaducados = await _databaseHelper.ObtenerInsumosProximosAVencer();
            ListaInsumosBajoStock = await _databaseHelper.ObtenerInsumosBajoStock();
        }
    }
}
