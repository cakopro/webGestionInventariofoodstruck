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

            public string MensajeError { get; set; }


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

                if (await _databaseHelper.RutExiste(ProveedorActual.Rut, ProveedorActual.Id))
                {
                    MensajeError = "Ya existe un proveedor registrado con este RUT.";
                    ModelState.AddModelError("ProveedorActual.Rut", "Ya existe un proveedor registrado con este RUT.");
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
