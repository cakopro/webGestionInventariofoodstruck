using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Text.Json;
using webGestionInventario.data;
using webGestionInventario.Model;

namespace webGestionInventario.Pages
{
    public class agregarEditarProductoModel : PageModel
    {
        private readonly DataBaseHelper _db;
        public agregarEditarProductoModel(IConfiguration config) => _db = new DataBaseHelper(config);

     
        [BindProperty] public int ProductoId { get; set; }
        [BindProperty] public string JsonReceta { get; set; } = "[]";
        [BindProperty] public string NombreProducto { get; set; }
        [BindProperty] public bool ProductoActualEstado { get; set; }
        public List<SelectListItem> Estados { get; set; }

        public List<Insumos> ListaInsumos { get; set; } = new();

        public async Task OnGet(int? id)
        {
            ListaInsumos = await _db.ObtenerInsumos();

            Estados = new List<SelectListItem>
        {
            new SelectListItem { Value = "true", Text = "Activo" },
            new SelectListItem { Value = "false", Text = "Desactivado" }
        };

            if (id.HasValue && id.Value > 0)
            {
               
                var producto = await _db.ObtenerProductoPorId(id.Value);
                if (producto != null)
                {
                    ProductoId = producto.Id;
                    NombreProducto = producto.Nombre;
                    ProductoActualEstado = producto.Estado;

                    var recetaOriginal = await _db.ObtenerIngredientesPorProducto(id.Value);
                    var recetaFormatoJs = recetaOriginal.Select(r => new
                    {
                        IdInsumo = r.Id,
                        NombreInsumo = r.Nombre,
                        Cantidad = r.Cantidad,
                        UnidadMedida = r.Unidad,
                        Precio = ListaInsumos.FirstOrDefault(i => i.Id == r.Id)?.PrecioUnitario ?? 0
                    }).ToList();

               
                    JsonReceta = JsonSerializer.Serialize(recetaFormatoJs);
                }
                else
                {
                    ProductoActualEstado = true;
                }
            }
        }

        public async Task<IActionResult> OnPostGuardar()
        {
            var listaFinal = JsonSerializer.Deserialize<List<TempReceta>>(JsonReceta);


            if (listaFinal != null && listaFinal.Count > 0)
            {
                var todosLosInsumos = await _db.ObtenerInsumos();
                decimal costoBase = 0;


                foreach (var item in listaFinal)
                {
                    var insumo = todosLosInsumos.FirstOrDefault(i => i.Id == item.IdInsumo);
                    if (insumo != null)
                    {
                        costoBase += (insumo.PrecioUnitario * (decimal)item.Cantidad);
                    }
                }

                decimal precioFinalConUtilidad = costoBase * 1.20m;

                if (ProductoId > 0)
                    await _db.UpdateProductoCalculado(ProductoId, NombreProducto, precioFinalConUtilidad, ProductoActualEstado, listaFinal);
                else
                    await _db.GuardarProductoCalculado(NombreProducto, precioFinalConUtilidad, listaFinal);

                return RedirectToPage("Productos");
            }
            return Page();
        }
    }
}