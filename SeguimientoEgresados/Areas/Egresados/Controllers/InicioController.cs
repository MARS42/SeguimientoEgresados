using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Egresados.Controllers
{
    [Area("Egresados")]
    public class InicioController : Controller
    {
        private Models.Usuario _usuario;

        private readonly SeguimientoEgresadosContext _context;
        private readonly INotificacionesService _notificaciones;
        
        public InicioController(SeguimientoEgresadosContext context, INotificacionesService notificaciones)
        {
            _context = context;
            _notificaciones = notificaciones;
        }
        
        [HttpGet, Route("~/Egresados/")]
        public async Task<IActionResult> Index()
        {
            /*_usuario = HttpContext.Session.Get<Usuario>("User");

            if (_usuario == null)
                return RedirectToAction("Index", "Acceso",new {area = "Egresados" });
            */
            ViewData["carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }
    }
}