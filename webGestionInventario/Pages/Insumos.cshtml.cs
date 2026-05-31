using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class InsumosModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public InsumosModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IList<Insumos> ListaInsumos { get; set; } = new List<Insumos>();

      
        [BindProperty(SupportsGet = true)]
        public bool MostrarInactivos { get; set; }

        public async Task OnGetAsync()
        {
           
            ListaInsumos = await _databaseHelper.ObtenerInsumos(MostrarInactivos);
        }

        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
           
            await _databaseHelper.DeleteInsumo(id);

          
            return RedirectToPage();
        }
    }
}
