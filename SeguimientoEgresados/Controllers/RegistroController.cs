using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace SeguimientoEgresados.Controllers
{
    public class RegistroController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;
        private readonly ICloudinaryService _cloudinary;
        
        public RegistroController(SeguimientoEgresadosContext context, ICloudinaryService _cloudinary)
        {
            this._context = context;
            this._cloudinary = _cloudinary;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string Tipo)
        {

            if (string.IsNullOrEmpty(Tipo))
                return View();
            
            if(Tipo.Equals("Empleador") || Tipo.Equals("Egresado"))
                return RedirectToAction(Tipo.Equals("Egresado") ? "Egresado" : "Empleador");

            return NotFound();
        }
        
        public IActionResult Egresado()
        {
            ViewData["Generos"] = new SelectList(_context.Generos, "Id", "Nombre");
            ViewData["EstadosCiviles"] = new SelectList(_context.EstadosCiviles, "Id", "Nombre");
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Egresado(RegistroEgresadoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            if (await ExisteEmail(model.Email))
                return View(model);
            
            var nombres = new SqlParameter("@nombre", model.Nombres);
            var ap1 = new SqlParameter("@apellido_paterno", model.ApellidoPaterno);
            var ap2 = new SqlParameter("@apellido_materno", model.ApellidoMaterno);
            var email = new SqlParameter("@email", model.Email);
            var password = new SqlParameter("@password", model.Password);
            var id_rol = new SqlParameter("@id_rol", 4);
            
            var url_img = new SqlParameter("@url_img", "No URL");
            var verificado = new SqlParameter("@verificado", "true");
            
            
            var id_generado = new SqlParameter("@id_generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            
            await _context.Database.ExecuteSqlRawAsync("exec AgregarUsuario @nombre, @apellido_paterno, @apellido_materno, @email, @password, @id_rol, @url_img, @verificado, @id_generado out", nombres, ap1, ap2, email, password, id_rol,url_img,verificado, id_generado);

            Console.WriteLine("Usuario agregado id: " + id_generado.Value);
            
            var egresado = new Egresado()
            {
                Colonia = model.Colonia,
                Cp = int.Parse(model.CodigoPostal),
                Domicilio = model.Domicilio,
                Estado = model.Estado,
                Municipio = model.Municipio,
                Pais = model.Pais,
                Telefono = model.Telefono,
                EstadoNacimiento = model.EstadoNacimiento,
                FechaEgreso = model.FechaEgreso,
                FechaNacimiento = model.FechaNacimiento,
                MunicipioNacimiento = model.MunicipioNacimiento,
                NControl = model.NoControl,
                PaisNacimiento = model.PaisNacimiento,
                IdCarrera = model.idCarrera,
                IdGenero = model.idGenero,
                IdEstadoCivil = model.idEstadoCivil,
                IdUsuario = Convert.ToInt32(id_generado.Value),
                FechaInicio = model.FechaInicio
            };

            await _context.Egresados.AddAsync(egresado);
            await _context.SaveChangesAsync();
            //return RedirectToAction("Index", "Acceso", new { Email = model.Email, Password = model.Password });
            return RedirectToActionPreserveMethod("Index", "Acceso",
                new AccesoViewModel() {Email = model.Email, Password = model.Password});
        }

        public IActionResult Empleador()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Empleador(RegistroEmpleadorViewModel model, IFormFile? imgperfil)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("modelomal");
                return View(model);
            }

            if (await ExisteEmail(model.Email))
                return View(model);

            var nombres = new SqlParameter("@nombre", model.Nombres);
            var ap1 = new SqlParameter("@apellido_paterno", model.ApellidoPaterno);
            var ap2 = new SqlParameter("@apellido_materno", model.ApellidoMaterno);
            var email = new SqlParameter("@email", model.Email);
            var password = new SqlParameter("@password", model.Password);
            var id_rol = new SqlParameter("@id_rol", 5);
            SqlParameter url_img;
            
            if (imgperfil != null)
            {
                string url = await _cloudinary.SubirImagenUsuario(imgperfil, model.Email);
                url_img = new SqlParameter("@url_img", url);
            }
            else
            {
                url_img = new SqlParameter("@url_img", "");
            }
            var verificado = new SqlParameter("@verificado", "false");
            
            var id_generado = new SqlParameter("@id_generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            
            var newUser = await _context.Database.ExecuteSqlRawAsync("exec AgregarUsuario @nombre, @apellido_paterno, @apellido_materno, @email, @password, @id_rol,@url_img, @verificado,@id_generado out", nombres, ap1, ap2, email, password, id_rol, url_img,verificado,id_generado);
            
            Console.WriteLine("Usuario agregado id: " + id_generado.Value);

            int userID = Convert.ToInt32(id_generado.Value);
            
            var empresa = new Empresa()
            {
                IdUsuario = userID,
                Colonia = model.Colonia,
                Cp = Convert.ToInt32(model.CodigoPostal),
                Domicilio = model.Domicilio,
                Estado = model.Estado,
                Municipio = model.Municipio,
                Nombre = model.NombreEmpresa,
                Pais = model.Pais,
                Rfc = model.RFC,
                Telefono = model.Telefono,
                Website = model.Website,
                CorreoEmpresa = model.EmailEmpresa,
                RazonSocial = model.RazonSocial,
                Descripcion = model.Descripcion
            };

            await _context.Empresas.AddAsync(empresa);
            await _context.SaveChangesAsync();

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(id_generado));

            //return RedirectToAction("Index", "Acceso", new { Email = model.Email, Password = model.Password });
            return RedirectToActionPreserveMethod("Index", "Acceso",
                new AccesoViewModel() {Email = model.Email, Password = model.Password});
        }

        [HttpGet]
        public async Task<IActionResult> ComprobarEmail(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower().Trim()));

            return Json(new { disponible = user == null });
        }

        public async Task<bool> ExisteEmail(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower().Trim()));
            return user != null;
        }
    }
}