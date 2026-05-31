using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace webGestionInventario.Pages
{
    public class VentasModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public VentasModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [BindProperty]
        public Venta NuevaVenta { get; set; } = new Venta();

        [BindProperty]
        public string DetallesJson { get; set; }

        
        public List<Producto> ListaProductos { get; set; } = new List<Producto>();

        public async Task OnGetAsync()
        {
            await CargarProductos();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (string.IsNullOrEmpty(DetallesJson))
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto a la venta.");
                await CargarProductos();
                return Page();
            }

            List<DetalleVenta> detalles;
            try
            {
                detalles = JsonSerializer.Deserialize<List<DetalleVenta>>(DetallesJson);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error al procesar los detalles.");
                await CargarProductos();
                return Page();
            }

            if (detalles == null || !detalles.Any())
            {
                ModelState.AddModelError(string.Empty, "La lista de productos está vacía.");
                await CargarProductos();
                return Page();
            }

            
            decimal subtotalGeneral = detalles.Sum(d => d.Subtotal);
            NuevaVenta.IVA = subtotalGeneral * 0.19m;
            NuevaVenta.Total = subtotalGeneral + NuevaVenta.IVA;

            try
            {
                await _databaseHelper.RegistrarVenta(NuevaVenta, detalles);
                return RedirectToPage("./Ventas", new { success = "true" });
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar: " + ex.Message);
                await CargarProductos();
                return Page();
            }
        }

        
        private async Task CargarProductos()
        {
            ListaProductos = await _databaseHelper.ObtenerProductos() ?? new List<Producto>();
        }
    }
}