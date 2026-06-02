using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class AgregarInsumoModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public AgregarInsumoModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [BindProperty]
        public Insumos InsumoNuevo { get; set; } = new Insumos();

        public List<Proveedores> ListaProveedores { get; set; }

        public async Task OnGetAsync()
        {
         
            ListaProveedores = await _databaseHelper.ObtenerProveedoresActivos();

        
            InsumoNuevo.FechaCaducidad = System.DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync()
        {
          
            ModelState.Remove("InsumoNuevo.NombreProveedor");
            ModelState.Remove("InsumoNuevo.Estado");

            if (InsumoNuevo.FechaCaducidad.Year < 1753)
            {
                InsumoNuevo.FechaCaducidad = System.DateTime.Today;
            }

            if (!ModelState.IsValid)
            {
                ListaProveedores = await _databaseHelper.ObtenerProveedoresActivos();
                return Page();
            }

            InsumoNuevo.Estado = true;
            await _databaseHelper.InsertInsumo(InsumoNuevo);

            return RedirectToPage("./Insumos");
        }
    }
}