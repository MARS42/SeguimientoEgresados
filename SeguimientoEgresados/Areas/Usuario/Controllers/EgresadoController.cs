using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class EgresadoController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;

        public EgresadoController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            //var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(usuario!.Id));
            //Console.WriteLine("cues: " + cuestionario);
            //ViewData["Cuestionario"] = cuestionario;
            return View(usuario);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(Models.Usuario model)
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));

            usuario.Nombre = model.Nombre;
            usuario.ApellidoPaterno = model.ApellidoPaterno;
            usuario.ApellidoMaterno = model.ApellidoMaterno;
            //usuario.Email = model.Email;
            
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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