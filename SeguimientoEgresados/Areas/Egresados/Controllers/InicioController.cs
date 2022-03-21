using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Egresados.Controllers
{
    [Area("Egresados")]
    public class InicioController : Controller
    {
        private Usuario _usuario;
        
        [HttpGet, Route("~/Egresados/")]
        public IActionResult Index()
        {
            /*_usuario = HttpContext.Session.Get<Usuario>("User");

            if (_usuario == null)
                return RedirectToAction("Index", "Acceso",new {area = "Egresados" });
*/
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }
    }
}