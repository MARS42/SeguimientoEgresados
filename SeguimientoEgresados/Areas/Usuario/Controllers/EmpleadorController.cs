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
        private ICloudinaryService _cloudinary;
        private readonly INotificacionesService _notificaciones;

        public EmpleadorController(SeguimientoEgresadosContext context, IGoogleSheetsService googleSheets, INotificacionesService notificaciones, ICloudinaryService cloudinary)
        {
            _context = context;
            this.googleSheets = googleSheets;
            _notificaciones = notificaciones;
            _cloudinary = cloudinary;
        }
        
        public async Task<IActionResult> Index()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
            //var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(usuario!.Id));
            
            //Console.WriteLine("cues: " + cuestionario);
            //ViewData["Cuestionario"] = cuestionario;
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);
            var user = await GetUsuario();

            if (user.EstaVerificado())
                return View(user);
            else
                return RedirectToAction("VerificarCuenta");
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
            
            if (!user.EstaVerificado())
                return RedirectToAction("VerificarCuenta");
            
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));

            ViewData["imgPerfil"] = user.UrlImg;
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {empresa.Nombre}");
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);

            return View(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> MiEmpresa(Empresa model, IFormFile? imgperfil)
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
            empresa.Descripcion = model.Descripcion;

            if (imgperfil != null)
            {
                string url = await _cloudinary.SubirImagenUsuario(imgperfil, user.Email);
                user.UrlImg = url;
                _context.Entry(user).State = EntityState.Modified;
            }

            _context.Entry(empresa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //return Ok(model);
            return RedirectToAction("MiEmpresa");
        }

        public async Task<IActionResult> PublicarEmpleo()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            
            if (!user.EstaVerificado())
                return RedirectToAction("VerificarCuenta");
            
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);
            
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Cuestionario(bool error = false)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            
            if (!user.EstaVerificado())
                return RedirectToAction("VerificarCuenta");
            
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
                ViewData["Error"] = new AvisoCuestionario("A?n no has respondido el cuestionario", "Parece que a?n no est? registrado el cuestionario, contestalo y al finalizar pulsa el bot?n ENVIAR del formulario", true);

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
                Console.WriteLine("Actualziaci?n: " + msg);
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
        
        [HttpPost]
        public async Task<IActionResult> NuevaVacante(VacanteViewModel model)
        {
            var usuario = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(usuario.Id));

            if (empresa == null)
                return NotFound();
            
            Vacante vacante = new Vacante()
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Funciones = model.Funciones,
                Requisitos = model.Requisitos,
                Ofertas = model.Ofertas,
                TipoContrato = model.TipoContrato,
                Modalidad = model.Modalidad,
                Horario = model.Horario,
                IdEmpresa = empresa.Id,
                Fecha = DateTime.Now
            };

            await _context.Vacantes.AddAsync(vacante);
            await _context.SaveChangesAsync();
            
            return Json(model);
        }

        public async Task<IActionResult> GetVacantes()
        {
            var usuario = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(usuario.Id));
            var query = 
                from vacante in _context.Vacantes 
                where vacante.IdEmpresa.Equals(empresa.Id) select vacante;

            var lista = new List<VacanteEmpleadorViewModel>();
            foreach (var vacante in await query.AsNoTracking().ToListAsync())
            {
                var postulantes = from postulante in _context.Postulantes
                    where postulante.IdVacante.Equals(vacante.Id)
                    select postulante;
                
                var p = await postulantes.AsNoTracking().ToListAsync();
                lista.Add(new VacanteEmpleadorViewModel()
                {
                    Id = vacante.Id,
                    Titulo = vacante.Titulo,
                    Fecha = vacante.Fecha,
                    Postulantes = p.Count
                });
            }
            
            return PartialView("_TablaVacantes", lista);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarVacante(int id)
        {
            var usuario = await GetUsuario();
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(usuario.Id));

            var vacante = await _context.Vacantes.Include(v => v.Postulantes).FirstOrDefaultAsync(v => v.Id.Equals(id));

            if (empresa.Id != vacante.IdEmpresa)
                return Json("Error");
            
            _context.Postulantes.RemoveRange(vacante.Postulantes);
            await _context.SaveChangesAsync();
            _context.Vacantes.Remove(vacante);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> VerVacante(int id)
        {
            var usuario = await GetUsuario();
            
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(usuario.Id));
            var vacante = await _context.Vacantes.Include(v => v.Postulantes)
                .ThenInclude(p => p.IdEgresadoNavigation).ThenInclude(e => e.IdUsuarioNavigation)
                .FirstOrDefaultAsync(v => v.Id.Equals(id));

            return PartialView(vacante);
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ViewData["Email"] = (await GetUsuario())!.Email;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(CambioPasswordViewModel model)
        {
            ViewData["Email"] = (await GetUsuario())!.Email;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = new SqlParameter("@email", model.Email);
            var oldPass = new SqlParameter("@oldPassword", model.OldPassword);
            var newPass = new SqlParameter("@newPassword", model.NewPassword);

            var status = new SqlParameter("@status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var newUser = await _context.Database.ExecuteSqlRawAsync("exec CambiarPassword @email, @oldPassword, @newPassword, @status out", email, oldPass, newPass, status);

            if ((int)status.Value < 0)
            {
                ViewData["Error"] = $"Contrase?a actual incorrecta ({status.Value})";
                return View(model);
            }

            TempData["info"] = "Contrase?a actualizada";
            return RedirectToAction("Index");
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

        public IActionResult VerificarCuenta()
        {
            return View();
        }

        private bool InsertarCuestionario(string sheetsResponse)
        {
            return sheetsResponse.Equals("Ok");
        }
    }
}
