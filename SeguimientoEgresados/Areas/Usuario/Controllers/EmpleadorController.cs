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
    public class EmpleadorController : Controller
    {
        private SeguimientoEgresadosContext _context;

        public EmpleadorController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> MiEmpresa()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {empresa.Nombre}");
            
            return View(empresa);
        }

        public IActionResult PublicarEmpleo()
        {
            return View();
        }
        
        public async Task<IActionResult> Cuestionario()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.Id.Equals(empresa!.IdCuestionario));

            return View(cuestionario);
        }
        
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }
    }
}