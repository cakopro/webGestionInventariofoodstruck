using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class AgregarEditarProveedorModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public AgregarEditarProveedorModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

       
        [BindProperty]
        public Proveedores ProveedorActual { get; set; } = new Proveedores();

       
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
             
                ProveedorActual = await _databaseHelper.ObtenerProveedorPorId(id.Value);
                if (ProveedorActual == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
         
            ModelState.Remove("ProveedorActual.Estado");

            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            if (ProveedorActual.Id == 0)
            {
                ProveedorActual.Estado = true; 
                await _databaseHelper.InsertProveedor(ProveedorActual);
            }
            
            else
            {
                await _databaseHelper.UpdateProveedor(ProveedorActual);
            }

        
            return RedirectToPage("./Proveedores");
        }
    }
}
