using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            ViewData["Generos"] = new SelectList(_context.Generos, "Id", "Nombre");
            ViewData["EstadosCiviles"] = new SelectList(_context.EstadosCiviles, "Id", "Nombre");
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {egresado.NControl}");
            
            return View(egresado);
        }
        
        [HttpPost]
        public async Task<IActionResult> MisDatos(Egresado model)
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));

            egresado.Colonia = model.Colonia;
            egresado.Cp = model.Cp;
            egresado.Domicilio = model.Domicilio;
            egresado.Estado = model.Estado;
            egresado.Municipio = model.Municipio;
            egresado.Pais = model.Pais;
            
            egresado.FechaEgreso = model.FechaEgreso;
            egresado.FechaNacimiento = model.FechaNacimiento;
            
            egresado.PaisNacimiento = model.PaisNacimiento;
            egresado.MunicipioNacimiento = model.MunicipioNacimiento;
            egresado.EstadoNacimiento = model.EstadoNacimiento;

            egresado.IdGenero = model.IdGenero;
            egresado.IdCarrera = model.IdCarrera;
            egresado.IdEstadoCivil = model.IdEstadoCivil;

            egresado.NControl = model.NControl;
            egresado.Telefono = model.Telefono;
            
            _context.Entry(egresado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //return Ok(model);
            return RedirectToAction("MisDatos");
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