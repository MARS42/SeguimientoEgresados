using System.Data;
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

        public async Task<IActionResult> MisDatos()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            ViewData["Generos"] = new SelectList(_context.Generos, "Id", "Nombre");
            ViewData["EstadosCiviles"] = new SelectList(_context.EstadosCiviles, "Id", "Nombre");
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, true);
            
            var editarEgresado = new RegistroEgresadoViewModel()
            {
                Colonia = egresado.Colonia,
                CodigoPostal = egresado.Cp + "",
                Domicilio = egresado.Domicilio,
                Estado = egresado.Estado,
                Municipio = egresado.Municipio,
                Pais = egresado.Pais,
                Telefono = egresado.Telefono,
                EstadoNacimiento = egresado.EstadoNacimiento,
                FechaEgreso = egresado.FechaEgreso ?? DateTime.Now,
                FechaNacimiento = egresado.FechaNacimiento ?? DateTime.Now,
                MunicipioNacimiento = egresado.MunicipioNacimiento,
                NoControl = egresado.NControl,
                PaisNacimiento = egresado.PaisNacimiento,
                idCarrera = egresado.IdCarrera,
                idGenero = egresado.IdGenero,
                idEstadoCivil = egresado.IdEstadoCivil,
                FechaInicio = egresado.FechaInicio ?? DateTime.Now
            };
            
            return View(editarEgresado);
        }
        
        [HttpPost]
        public async Task<IActionResult> MisDatos(RegistroEgresadoViewModel model)
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var user = await GetUsuario();
            var egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));

            egresado.Colonia = model.Colonia;
            egresado.Cp = int.Parse(model.CodigoPostal);
            egresado.Domicilio = model.Domicilio;
            egresado.Estado = model.Estado;
            egresado.Municipio = model.Municipio;
            egresado.Pais = model.Pais;
            
            egresado.FechaEgreso = model.FechaEgreso;
            egresado.FechaNacimiento = model.FechaNacimiento;
            
            egresado.PaisNacimiento = model.PaisNacimiento;
            egresado.MunicipioNacimiento = model.MunicipioNacimiento;
            egresado.EstadoNacimiento = model.EstadoNacimiento;

            egresado.IdGenero = model.idGenero;
            egresado.IdCarrera = model.idCarrera;
            egresado.IdEstadoCivil = model.idEstadoCivil;

            egresado.NControl = model.NoControl;
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
                ViewData["Error"] = new AvisoCuestionario("A??n no has respondido el cuestionario", "Parece que a??n no est?? registrado el cuestionario, contestalo y al finalizar pulsa el bot??n ENVIAR del formulario", true);
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
                Console.WriteLine("Actualziaci??n: " + msg);
                if (!InsertarCuestionario(msg))
                    return RedirectToAction("Cuestionario", new { error = true });
                
                var id_usuario = new SqlParameter("@id_usuario", user!.Id);
                await _context.Database.ExecuteSqlRawAsync("exec ActualizarCuestionario @id_usuario", id_usuario);
            }
            return RedirectToAction("Cuestionario");
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
                ViewData["Error"] = $"Contrase??a actual incorrecta ({status.Value})";
                return View(model);
            }

            TempData["info"] = "Contrase??a actualizada";
            return RedirectToAction("Index");
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
        //         return new AvisoCuestionario("Bienvenido al seguimiento de egresados", "El sistema de seguimiento egresados centra y ofrece su informaci??n para el conocimiento y beneficio de todos.<br/>Esto es posible mediante la obtenci??n de informaci??n de los miembros que la integran.Por ello, debes realizar un cuestionario inicial y completar tu registro.");
        //
        //     if (DateTime.Compare(cuestionario.ProximaAplicacion, DateTime.Now) <= 0)
        //         return new AvisoCuestionario("Actualizaci??n de cuestionario", $"Tu ??ltima aplicaci??n del cuestinario fue el {cuestionario.UltimaAplicacion.Value.ToString()}, puedes volver a aplicarlo para mantener tus datos actualizados.");
        //
        //     return null;
        // }
        
        private bool InsertarCuestionario(string sheetsResponse)
        {
            return sheetsResponse.Equals("Ok");
        }
    }
}