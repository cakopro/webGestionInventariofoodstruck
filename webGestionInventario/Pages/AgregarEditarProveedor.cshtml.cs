using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class AgregarEditarProveedorModel : PageModel
    {
        private readonly DataBaseHelper _dbHelper;
        
        public AgregarEditarProveedorModel(DataBaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [BindProperty]
        public Proveedores NuevoProveedor { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _dbHelper.InsertProveedor(NuevoProveedor);

            return RedirectToPage("Proveedores");
        }
    }
}
