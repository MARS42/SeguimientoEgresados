using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
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
            ViewData["carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _GetEgresados()
        {
            var egresadosCarrera = _context.Egresados.Include(c => c.IdCarreraNavigation).AsNoTracking();
            
            var egresados =
                from egresado in egresadosCarrera
                join usuario in _context.Usuarios on egresado.IdUsuario equals usuario.Id into matches
                from match in matches
                select new EgresadoPublicoViewModel()
                {
                    Carrera = egresado.IdCarreraNavigation.Nombre,
                    Nombres = match.Nombre,
                    ApeM = match.ApellidoMaterno,
                    ApeP = match.ApellidoPaterno,
                    NoControl = egresado.NControl,
                    FechaInicio = egresado.FechaInicio,
                    FechaFin = egresado.FechaEgreso
                };

            return PartialView(await ListaPaginada<EgresadoPublicoViewModel>.CreateAsync(egresados.AsNoTracking(), 1, 100));
        }
    }
}