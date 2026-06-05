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

        [BindProperty(SupportsGet = true)]
        public bool MostrarInactivos { get; set; }

        public async Task OnGetAsync()
        {
            ListaProductos = await _databaseHelper.ObtenerProductos(MostrarInactivos);
        }

        public async Task<JsonResult> OnGetCargarIngredientesAsync(int id)
        {
            var ingredientes = await _databaseHelper.ObtenerIngredientesPorProducto(id);

            return new JsonResult(ingredientes);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _databaseHelper.DeleteProductos(id);
            return RedirectToPage();
        }
    }
}
