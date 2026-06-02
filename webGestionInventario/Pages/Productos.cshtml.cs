using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class ProductosModel : PageModel
    {

        private readonly DataBaseHelper _databaseHelper;

        public ProductosModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public List<Producto> ListaProductos { get; set; } = new List<Producto>();

        public async Task OnGetAsync()
        {
            ListaProductos = await _databaseHelper.ObtenerProductos() ?? new List<Producto>();
        }

        public async Task<JsonResult> OnGetCargarIngredientesAsync(int id)
        {
            var ingredientes = await _databaseHelper.ObtenerIngredientesPorProducto(id);

            return new JsonResult(ingredientes);
        }
    }
}
