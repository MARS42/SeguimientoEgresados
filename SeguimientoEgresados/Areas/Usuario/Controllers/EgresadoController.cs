using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class EgresadoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MisDatos()
        {
            return View();
        }

        public IActionResult Cuestionario()
        {
            return View();
        }
        
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }
    }
}