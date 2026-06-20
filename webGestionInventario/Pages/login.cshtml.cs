using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webGestionInventario.data;
using System.Threading.Tasks;

namespace webGestionInventario.Pages
{
    public class loginModel : PageModel
    {
        private readonly DataBaseHelper _databaseHelper;

        public loginModel(DataBaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

       
        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contrasena { get; set; }

        public string MensajeError { get; set; }

        public IActionResult OnGet(string? origen)
        {
            if (origen == "qr")
            {
                return RedirectToPage("/Menu");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
       
            bool esValido = await _databaseHelper.ValidarUsuario(NombreUsuario, Contrasena);

            if (esValido)
            {
              
                return RedirectToPage("/Index");
            }
            else
            {
              
                MensajeError = "Usuario o contraseña incorrectos.";
                return Page();
            }
        }
    }
}