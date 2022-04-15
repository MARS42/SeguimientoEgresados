using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Authorize(Roles = "Empleador")]
    [Area("Usuario")]
    public class EmpleadorController : Controller
    {
        private SeguimientoEgresadosContext _context;
        private IGoogleSheetsService googleSheets;
        private readonly INotificacionesService _notificaciones;

        public EmpleadorController(SeguimientoEgresadosContext context, IGoogleSheetsService googleSheets, INotificacionesService notificaciones)
        {
            _context = context;
            this.googleSheets = googleSheets;
            _notificaciones = notificaciones;
        }
        
        public async Task<IActionResult> Index()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            //var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(usuario!.Id));
            
            //Console.WriteLine("cues: " + cuestionario);
            //ViewData["Cuestionario"] = cuestionario;
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);

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
        
        public async Task<IActionResult> MiEmpresa()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {empresa.Nombre}");
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);

            return View(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> MiEmpresa(Empresa model)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));

            empresa.Colonia = model.Colonia;
            empresa.Cp = model.Cp;
            empresa.Domicilio = model.Domicilio;
            empresa.Estado = model.Estado;
            empresa.Municipio = model.Municipio;
            empresa.Nombre = model.Nombre;
            empresa.Pais = model.Pais;
            empresa.Rfc = model.Rfc;
            empresa.RazonSocial = model.RazonSocial;
            empresa.Website = model.Website;
            empresa.CorreoEmpresa = model.CorreoEmpresa;
            empresa.Telefono = model.Telefono;
            
            _context.Entry(empresa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //return Ok(model);
            return RedirectToAction("MiEmpresa");
        }

        public async Task<IActionResult> PublicarEmpleo()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Cuestionario(bool error = false)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(empresa!.IdUsuario));

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
                msg = await googleSheets.VerificarCuestionario(false, user!.Email);
                
                if (!InsertarCuestionario(msg))
                    return RedirectToAction("Cuestionario", new { error = true });
                
                var id_usuario = new SqlParameter("@id_usuario", user!.Id);
                await _context.Database.ExecuteSqlRawAsync("exec CrearCuestionario @id_usuario", id_usuario);
            }
            else
            {
                msg = await googleSheets.VerificarCuestionario(false, user!.Email, cuestionario.ProximaAplicacion);
                Console.WriteLine("Actualziación: " + msg);
                if (!InsertarCuestionario(msg))
                    return RedirectToAction("Cuestionario", new { error = true });
                
                var id_usuario = new SqlParameter("@id_usuario", user!.Id);
                await _context.Database.ExecuteSqlRawAsync("exec ActualizarCuestionario @id_usuario", id_usuario);
            }
            return RedirectToAction("Cuestionario");
        }

        [HttpGet]
        public IActionResult NuevaVacante()
        {
            return PartialView("_NuevaVacante");
        }
        
        private async Task<Models.Usuario?> GetUsuario()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
        
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Inicio", new { area="" });
        }

        private bool InsertarCuestionario(string sheetsResponse)
        {
            return sheetsResponse.Equals("Ok");
        }
    }
}
