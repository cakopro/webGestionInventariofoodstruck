using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class ProveedoresModel : PageModel
    {
        private readonly DataBaseHelper _db;
        public List<Proveedores> ListaProveedores { get; set; }

        public ProveedoresModel(DataBaseHelper db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            ListaProveedores = await _db.ObtenerProveedores();
        }
    }
}
