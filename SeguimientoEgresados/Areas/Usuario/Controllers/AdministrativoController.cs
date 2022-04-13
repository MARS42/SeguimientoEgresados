using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class AdministrativoController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;

        public AdministrativoController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            return View(usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("Module");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }
    }
}