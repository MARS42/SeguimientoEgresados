using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class EmpleadorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> MisEmpresa()
        {
            return View();
        }

        public IActionResult PublicarEmpleo()
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