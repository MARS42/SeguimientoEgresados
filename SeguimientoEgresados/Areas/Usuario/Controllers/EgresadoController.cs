using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Authorize(Roles = "Egresado")]
    [Area("Usuario")]
    public class EgresadoController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;
        private readonly IGoogleSheetsService _sheets;
        private readonly INotificacionesService _notificaciones;

        public EgresadoController(SeguimientoEgresadosContext context, IGoogleSheetsService sheets, INotificacionesService notificaciones)
        {
            _context = context;
            _sheets = sheets;
            _notificaciones = notificaciones;
        }
        
        public async Task<IActionResult> Index()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            
            //var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(usuario!.Id));
            //Console.WriteLine("cues: " + cuestionario);
            //ViewData["Cuestionario"] = cuestionario;
            
            await _notificaciones.VerificarCuestionario(HttpContext, ViewData, true);
            
            return View(await GetUsuario());
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(Models.Usuario model)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            var usuario = await GetUsuario();

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
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            ViewData["Generos"] = new SelectList(_context.Generos, "Id", "Nombre");
            ViewData["EstadosCiviles"] = new SelectList(_context.EstadosCiviles, "Id", "Nombre");
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {egresado.NControl}");
            await _notificaciones.VerificarCuestionario(HttpContext, ViewData, true);
            
            return View(egresado);
        }
        
        [HttpPost]
        public async Task<IActionResult> MisDatos(Egresado model)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
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

        [HttpGet]
        public async Task<IActionResult> Cuestionario(bool error = false)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(egresado!.IdUsuario));

            if (cuestionario != null && DateTime.Compare(cuestionario.ProximaAplicacion, DateTime.Now) <= 0)
            {
                ViewData["Modo"] = "Editar";
                ViewData["ModoBoton"] = "He actualizado el cuestionario";
            }
            else if (cuestionario == null)
            {
                ViewData["Modo"] = "Crear";
                ViewData["ModoBoton"] = "He finalizado el cuestionario";
            }
            else
                ViewData["Modo"] = "Hecho";
            
            if (error)
                ViewData["Error"] = new AvisoCuestionario("Aún no has respondido el cuestionario", "Parece que aún no está registrado el cuestionario, contestalo y al finalizar pulsa el botón ENVIAR del formulario", true);
            return View(cuestionario);
        }
        
        [HttpPost]
        public async Task<IActionResult> Cuestionario(String Verificar)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(user.Id));

            string msg = "";
            if (cuestionario == null)
            {
                msg = await _sheets.VerificarCuestionario(true, user!.Email);
                
                if (!InsertarCuestionario(msg))
                    return RedirectToAction("Cuestionario", new { error = true });
                
                var id_usuario = new SqlParameter("@id_usuario", user!.Id);
                await _context.Database.ExecuteSqlRawAsync("exec CrearCuestionario @id_usuario", id_usuario);
            }
            else
            {
                msg = await _sheets.VerificarCuestionario(true, user!.Email, cuestionario.ProximaAplicacion);
                Console.WriteLine("Actualziación: " + msg);
                if (!InsertarCuestionario(msg))
                    return RedirectToAction("Cuestionario", new { error = true });
                
                var id_usuario = new SqlParameter("@id_usuario", user!.Id);
                await _context.Database.ExecuteSqlRawAsync("exec ActualizarCuestionario @id_usuario", id_usuario);
            }
            return RedirectToAction("Cuestionario");
        }

        private async Task<Models.Usuario?> GetUsuario()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
        
        public async Task<IActionResult> CerrarSesion()
        {
            //HttpContext.Session.Remove("User");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Inicio", new { area="" });
        }
        
        // private async Task<AvisoCuestionario> Aviso(int idUsuario)
        // {
        //     Cuestionario? cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(idUsuario));
        //     if(cuestionario == null)
        //         return new AvisoCuestionario("Bienvenido al seguimiento de egresados", "El sistema de seguimiento egresados centra y ofrece su información para el conocimiento y beneficio de todos.<br/>Esto es posible mediante la obtención de información de los miembros que la integran.Por ello, debes realizar un cuestionario inicial y completar tu registro.");
        //
        //     if (DateTime.Compare(cuestionario.ProximaAplicacion, DateTime.Now) <= 0)
        //         return new AvisoCuestionario("Actualización de cuestionario", $"Tu última aplicación del cuestinario fue el {cuestionario.UltimaAplicacion.Value.ToString()}, puedes volver a aplicarlo para mantener tus datos actualizados.");
        //
        //     return null;
        // }
        
        private bool InsertarCuestionario(string sheetsResponse)
        {
            return sheetsResponse.Equals("Ok");
        }
    }
}