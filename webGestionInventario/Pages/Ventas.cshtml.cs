using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using webGestionInventario.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

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

        public List<VentaHistorialViewModel> HistorialVentas { get; set; } = new List<VentaHistorialViewModel>();

        public async Task OnGetAsync()
        {
            await CargarProductos();
            await CargarHistorial();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(DetallesJson) || DetallesJson == "[]")
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto a la lista antes de finalizar la venta.");
                await CargarProductos();
                await CargarHistorial();
                return Page();
            }

            List<DetalleVenta> detalles = JsonSerializer.Deserialize<List<DetalleVenta>>(DetallesJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (detalles == null || !detalles.Any())
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto.");
                await CargarProductos();
                await CargarHistorial();
                return Page();
            }

            decimal subtotalGeneral = detalles.Sum(d => d.Subtotal);
            NuevaVenta.IVA = subtotalGeneral * 0.19m;
            NuevaVenta.Total = subtotalGeneral + NuevaVenta.IVA;

            try
            {
               
                await _databaseHelper.RegistrarVenta(NuevaVenta, detalles);

                // EN ESTA PARTE DEBEN DE PONER SUS ACCESS TOKEN, QUE SE CREARON CON ANTERIORIDAD AL TENER SUS CUENTAS EN MERCADO LIBRE DEVELOPERS!!
                MercadoPagoConfig.AccessToken = "TU_TOKEN_AQUI";

                var itemsMercadoPago = new List<PreferenceItemRequest>();

                foreach (var d in detalles)
                {
                    itemsMercadoPago.Add(new PreferenceItemRequest
                    {
                        Title = $"Producto ID: {d.IdProducto}",
                        Quantity = d.Cantidad,
                        CurrencyId = "CLP",
                        UnitPrice = (decimal)d.Precio 
                    });
                }
     
                if (NuevaVenta.IVA > 0)
                {
                    itemsMercadoPago.Add(new PreferenceItemRequest
                    {
                        Title = "IVA (19%)",
                        Quantity = 1,
                        CurrencyId = "CLP",
                        UnitPrice = NuevaVenta.IVA
                    });
                }

                // Definir los BackUrls explícitamente primero
                var preferenceRequest = new PreferenceRequest
                {
                    Items = itemsMercadoPago,
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = "http://localhost:5215/Ventas",
                        Failure = "http://localhost:5215/Ventas",
                        Pending = "http://localhost:5215/Ventas"
                    }
                    // ¡BORRA O COMENTA LAS LÍNEAS DE AutoReturn Y BinaryMode SI SIGUEN AHÍ!
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(preferenceRequest);

                // 2. Redireccionamos
                return Redirect(preference.InitPoint);

                return Redirect(preference.InitPoint);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al procesar la venta: {ex.Message}");
                await CargarProductos();
                await CargarHistorial();
                return Page();
            }
        }

        private async Task CargarProductos()
        {
            ListaProductos = await _databaseHelper.ObtenerProductos() ?? new List<Producto>();
        }

        private async Task CargarHistorial()
        {
            var listaTipada = await _databaseHelper.ObtenerHistorialVentas() ?? new List<VentaHistorialDto>();

            if (!listaTipada.Any())
            {
                HistorialVentas = new List<VentaHistorialViewModel>();
                return;
            }

            HistorialVentas = listaTipada
                .GroupBy(v => v.IdVenta)
                .Select(g => {
                    var primeraFila = g.First();
                    string nombreCliente = string.IsNullOrWhiteSpace(primeraFila.Cliente) ? "Consumidor Final" : primeraFila.Cliente;

                    return new VentaHistorialViewModel
                    {
                        Fecha = primeraFila.Fecha,
                        Cliente = nombreCliente,
                        ProductosResumen = string.Join(", ", g
                            .GroupBy(x => x.Producto)
                            .Select(p => $"{p.Sum(item => item.Cantidad)}x {p.Key}")),
                        Total = primeraFila.TotalVenta
                    };
                })
                .ToList();
        }

        public class VentaHistorialViewModel
        {
            public DateTime Fecha { get; set; }
            public string Cliente { get; set; }
            public string ProductosResumen { get; set; }
            public decimal Total { get; set; }
        }
    }
}