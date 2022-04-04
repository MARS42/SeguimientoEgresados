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
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class EmpleadorController : Controller
    {
        private SeguimientoEgresadosContext _context;
        private IGoogleSheetsService googleSheets;

        public EmpleadorController(SeguimientoEgresadosContext context, IGoogleSheetsService googleSheets)
        {
            _context = context;
            this.googleSheets = googleSheets;
        }
        
        public async Task<IActionResult> Index()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(user!.Id));
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
        
        public async Task<IActionResult> MiEmpresa()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {empresa.Nombre}");
            
            return View(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> MiEmpresa(Empresa model)
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
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

        public IActionResult PublicarEmpleo()
        {
            return View();
        }
        
        public async Task<IActionResult> Cuestionario()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            var cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(empresa!.IdUsuario));

            return View(cuestionario);
        }

        [HttpPost]
        public async Task<IActionResult> Cuestionario(String Verificar)
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            string msg = await googleSheets.VerificarCuestionario(user!.Email);
            
            if (!InsertarCuestionario(msg))
            {
                return View();
            }
            
            var id_usuario = new SqlParameter("@id_usuario", user!.Id);
            
            await _context.Database.ExecuteSqlRawAsync("exec CrearCuestionario @id_usuario", id_usuario);
            
            //return View();
            return RedirectToAction("Cuestionario");
            return Ok(await googleSheets.VerificarCuestionario(user!.Email));
        }
        
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }

        private bool InsertarCuestionario(string sheetsResponse)
        {
            return sheetsResponse.Equals("Ok");
        }
    }
}
