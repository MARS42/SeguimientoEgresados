using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
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
        
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> MiEmpresa()
        {
            Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user!.Id));
            
            Console.WriteLine($"User id: {user.Id}, empresa id: {empresa.Nombre}");
            
            return View(empresa);
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