using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Egresados.Controllers
{
    [Area("Egresados")]
    public class InicioController : Controller
    {
        private Models.Usuario _usuario;
        
        private readonly INotificacionesService _notificaciones;
        
        public InicioController(INotificacionesService notificaciones)
        {
            _notificaciones = notificaciones;
        }
        
        [HttpGet, Route("~/Egresados/")]
        public async Task<IActionResult> Index()
        {
            /*_usuario = HttpContext.Session.Get<Usuario>("User");

            if (_usuario == null)
                return RedirectToAction("Index", "Acceso",new {area = "Egresados" });
*/
            await _notificaciones.VerificarCuestionario(HttpContext, ViewData, false);
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }
    }
}