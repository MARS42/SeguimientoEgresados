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
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Controllers
{
    public class AccesoController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;
        
        public AccesoController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirigirPerfil(View());
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string Email, string Password)
        {
            var email = new SqlParameter("@email", Email);
            var password = new SqlParameter("@password", Password);
            var idUsuario = new SqlParameter("@id_usuario", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var users = await _context.Database.ExecuteSqlRawAsync("exec AccesoUsuario @email, @password, @id_usuario out", email, password, idUsuario);
                
            Console.WriteLine("Id fount: " + idUsuario.Value);
                
            var oUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id.Equals(Convert.ToInt32(idUsuario.Value)));
                
            if (oUser == null)
            {
                ViewBag.Error = "Usuario o contraseña invalida";
                return View();
            }

            //Session["User"] = oUser;
            HttpContext.Session.Set<Usuario>("User", oUser);
                
            return RedirigirPerfil(View());
        }


        [HttpPost]
        public async Task<IActionResult> Index(AccesoViewModel model)
        {
            try
            {
                //FromSqlRaw = queries SELECT
                //ExecuteSqlRaw = INSERT, DELETE, UPDATE
                // -2 = Correo malo, -1 =
                var email = new SqlParameter("@email", model.Email);
                var password = new SqlParameter("@password", model.Password);
                var idUsuario = new SqlParameter("@id_usuario", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                var users = await _context.Database.ExecuteSqlRawAsync("exec AccesoUsuario @email, @password, @id_usuario out", email, password, idUsuario);
                
                Console.WriteLine("Id fount: " + idUsuario.Value);
                
                var oUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id.Equals(Convert.ToInt32(idUsuario.Value)));
                
                if (oUser == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View(model);
                }

                //Session["User"] = oUser;
                HttpContext.Session.Set<Usuario>("User", oUser);
                
                return RedirigirPerfil(View());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        private IActionResult RedirigirPerfil(IActionResult defaultView)
        {
            Usuario? user = HttpContext.Session.Get<Usuario>("User"); 
            if (user != null)
            {
                switch (user.IdRol)
                {
                    case 4:         //Egresado
                        return RedirectToAction("Index", "Egresado", new {area = "Usuario"});
                    case 5:         //Empleador
                        return RedirectToAction("Index", "Empleador", new {area = "Usuario"});
                }
            }

            return defaultView;
        }
    }
}