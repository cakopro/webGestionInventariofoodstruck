using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class ProveedoresModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public ProveedoresModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Proveedores> ListaProveedores { get; set; }

      
        [BindProperty(SupportsGet = true)]
        public bool MostrarInactivos { get; set; }

        public async Task OnGetAsync()
        {
            ListaProveedores = await _databaseHelper.ObtenerProveedores(MostrarInactivos);
        }

     
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _databaseHelper.DeleteProveedor(id);
            return RedirectToPage();
        }
    }
}