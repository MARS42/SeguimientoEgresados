using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Controllers
{
    public class RegistroController : Controller
    {
        private SeguimientoEgresadosContext _context;
        
        public RegistroController(SeguimientoEgresadosContext context)
        {
            this._context = context;
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
        public async Task<IActionResult> Egresado(RegistroEgresadoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var nombres = new SqlParameter("@nombre", model.Nombres);
            var ap1 = new SqlParameter("@apellido_paterno", model.ApellidoPaterno);
            var ap2 = new SqlParameter("@apellido_materno", model.ApellidoMaterno);
            var email = new SqlParameter("@email", model.Email);
            var password = new SqlParameter("@password", model.Password);
            var id_rol = new SqlParameter("@id_rol", 4);
            var id_generado = new SqlParameter("@id_generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            
            await _context.Database.ExecuteSqlRawAsync("exec AgregarUsuario @nombre, @apellido_paterno, @apellido_materno, @email, @password, @id_rol,@id_generado out", nombres, ap1, ap2, email, password, id_rol, id_generado);

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
                
            };
            
            return RedirectToAction("Index", "Acceso", new { Email = model.Email, Password = model.Password });
        }

        public IActionResult Empleador()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Empleador(RegistroEmpleadorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nombres = new SqlParameter("@nombre", model.Nombres);
            string[] apellidos = model.Apellidos.Split(" ");
            var ap1 = new SqlParameter("@apellido_paterno", apellidos[0]);
            var ap2 = new SqlParameter("@apellido_materno", apellidos[1]);
            var email = new SqlParameter("@email", model.Email);
            var password = new SqlParameter("@password", model.Password);
            var id_rol = new SqlParameter("@id_rol", 5);
            var id_generado = new SqlParameter("@id_generado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            
            var newUser = await _context.Database.ExecuteSqlRawAsync("exec AgregarUsuario @nombre, @apellido_paterno, @apellido_materno, @email, @password, @id_rol,@id_generado out", nombres, ap1, ap2, email, password, id_rol, id_generado);
            
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
                RazonSocial = model.RazonSocial
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Acceso", new { Email = model.Email, Password = model.Password });
        }
    }
}