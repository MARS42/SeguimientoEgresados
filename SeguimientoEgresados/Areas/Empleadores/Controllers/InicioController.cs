using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Services;

namespace SeguimientoEgresados.Areas.Empleadores.Controllers
{
    [Area("Empleadores")]
    public class InicioController : Controller
    {
        private readonly INotificacionesService _notificaciones;
        
        public InicioController(INotificacionesService notificaciones)
        {
            _notificaciones = notificaciones;
        }
        
        [HttpGet, Route("~/Empleadores/")]
        public async Task<IActionResult> Index()
        {
            await _notificaciones.VerificarCuestionario(HttpContext, ViewData, false);
            return View();
        }
    }
}