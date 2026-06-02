using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using webGestionInventario.Model;
using webGestionInventario.data;

namespace webGestionInventario.Pages
{
    public class agregarEditarProductoModel : PageModel
    {
        private readonly DataBaseHelper _db;
        public agregarEditarProductoModel(IConfiguration config) => _db = new DataBaseHelper(config);

        [BindProperty] public string JsonReceta { get; set; } = "[]";
        [BindProperty] public string NombreProducto { get; set; }

        public List<Insumos> ListaInsumos { get; set; } = new();

        public async Task OnGet()
        {
            ListaInsumos = await _db.ObtenerInsumos();
        }

        public async Task<IActionResult> OnPostGuardar()
        {
            var listaFinal = JsonSerializer.Deserialize<List<TempReceta>>(JsonReceta);

            if (listaFinal != null && listaFinal.Count > 0)
            {
                
                var todosLosInsumos = await _db.ObtenerInsumos();
                decimal precioCalculado = 0;

                // 2. Calculamos el precio: Suma(Cantidad * PrecioUnitario)
                foreach (var item in listaFinal)
                {
                    var insumo = todosLosInsumos.FirstOrDefault(i => i.Id == item.IdInsumo);
                    if (insumo != null)
                    {
                        precioCalculado += (insumo.PrecioUnitario * item.Cantidad);
                    }
                }

               
                await _db.GuardarProductoCalculado(NombreProducto, precioCalculado, listaFinal);
            }

            return RedirectToPage("Productos");
        }
    }
}