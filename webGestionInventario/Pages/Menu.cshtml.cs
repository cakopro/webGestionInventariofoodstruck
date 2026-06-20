using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class MenuModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public MenuModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Producto> ListaProductos { get; set; } = new List<Producto>();

        public void OnGet()
        {
            ListaProductos = _databaseHelper.ObtenerProductos(false).Result;
        }
    }
}
