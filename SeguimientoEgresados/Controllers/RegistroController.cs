using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;

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
        public IActionResult Index(string Tipo){
            
            if(Tipo.Equals("Empleador") || Tipo.Equals("Egresado"))
                return RedirectToAction(Tipo.Equals("Egresado") ? "Egresado" : "Empleador");

            return NotFound();
        }
        
        public IActionResult Egresado()
        {
            return View();
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

            var empresa = new Empresa()
            {
                IdUsuario = Convert.ToInt32(id_generado.Value),
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

            return RedirectToAction("Index", "Acceso");
        }
    }
}