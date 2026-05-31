using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class EditarInsumoModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public EditarInsumoModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [BindProperty]
        public Insumos InsumoEditar { get; set; }

    
        public List<Proveedores> ListaProveedores { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            InsumoEditar = await _databaseHelper.ObtenerInsumoPorId(id);

            if (InsumoEditar == null)
            {
                return NotFound();
            }

         
            ListaProveedores = await _databaseHelper.ObtenerProveedores();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (InsumoEditar.FechaCaducidad.Year < 1753)
            {
                InsumoEditar.FechaCaducidad = System.DateTime.Today;
            }

            if (!ModelState.IsValid)
            {
               
                ListaProveedores = await _databaseHelper.ObtenerProveedores();
                return Page();
            }

          
            await _databaseHelper.UpdateInsumo(InsumoEditar);

            return RedirectToPage("./Insumos");
        }
    }
}