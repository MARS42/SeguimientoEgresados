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
            ViewData["SidebarItem"] = 1;
            return View(usuario);
        }

        public async Task<IActionResult> General()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Egresados()
        {
            ViewData["SidebarItem"] = 2;
            return PartialView(await _context.Egresados.ToListAsync());
        }
        
        public async Task<IActionResult> Empleadores()
        {
            ViewData["SidebarItem"] = 3;
            return PartialView(await _context.Empresas.ToListAsync());
        }
        
        public async Task<IActionResult> CambiosPassword()
        {
            ViewData["SidebarItem"] = 4;
            return PartialView();
        }
        
        public async Task<IActionResult> Reportes()
        {
            ViewData["SidebarItem"] = 5;
            return PartialView();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("Module");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }
    }
}