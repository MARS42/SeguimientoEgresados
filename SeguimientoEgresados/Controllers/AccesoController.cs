using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        
        // [HttpGet]
        // public IActionResult Index()
        // {
        //     return RedirigirPerfil(View());
        // }
        
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            // if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            //     return RedirigirPerfil(View());
            //
            // var email = new SqlParameter("@email", Email);
            // var password = new SqlParameter("@password", Password);
            // var idUsuario = new SqlParameter("@id_usuario", SqlDbType.Int)
            // {
            //     Direction = ParameterDirection.Output
            // };

            //var users = await _context.Database.ExecuteSqlRawAsync("exec AccesoUsuario @email, @password, @id_usuario out", email, password, idUsuario);
                
            //Console.WriteLine("Id fount: " + idUsuario.Value);
                
            //var oUser = await _context.Usuarios
            //    .FirstOrDefaultAsync(u => u.Id.Equals(Convert.ToInt32(idUsuario.Value)));
                
            // if (oUser == null)
            // {
            //     ViewBag.Error = "Usuario o contraseña invalida";
            //     return View();
            // }
            //
            // //Session["User"] = oUser;
            // HttpContext.Session.Set<Usuario>("User", oUser);

            ViewData["ReturnUrl"] = returnUrl;
            return await RedirigirPerfil(View());
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

                await _context.Database.ExecuteSqlRawAsync("exec AccesoUsuario @email, @password, @id_usuario out", email, password, idUsuario);
                
                Console.WriteLine("Id found: " + idUsuario.Value);
                
                var oUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id.Equals(Convert.ToInt32(idUsuario.Value)));
                
                if (oUser == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View(model);
                }

                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id.Equals(oUser.IdRol));
                var query = from modulo in _context.Modulos
                    join operacion in _context.Operaciones on modulo.Id equals operacion.IdModulo
                    join rolOp in _context.RolOperacions on operacion.Id equals rolOp.IdOperacion into matches
                    from match in matches.DefaultIfEmpty()
                    where match.IdRol == rol.Id
                    select modulo;
                
                List<Claim> claims = new ()
                {
                    new Claim(ClaimTypes.Name, oUser.Nombre),
                    new Claim(ClaimTypes.Role, rol!.Nombre),
                    new Claim(ClaimTypes.Email, oUser.Email),
                    new Claim(ClaimTypes.System, query.First().Nombre)
                };
                ClaimsIdentity claimsIdentity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
                
                //HttpContext.Session.Set<Usuario>("User", oUser);
                
                switch (rol.Id)
                {
                    case 1:
                    case 2:
                    case 3:
                        return RedirectToAction("Index", "Administrativo", new { area = "Usuario" });
                    case 4:         //Egresado
                        return RedirectToAction("Index", "Egresado", new { area = "Usuario" });
                    case 5:         //Empleador
                        return RedirectToAction("Index", "Empleador", new { area = "Usuario" });
                }
                
                //if(string.IsNullOrEmpty(returnUrl))
                return await RedirigirPerfil(View());
                
                //return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        private async Task<IActionResult> RedirigirPerfil(IActionResult defaultView)
        {
            //Usuario? user = HttpContext.Session.Get<Usuario>("User");
            
            if (!User.Identity.IsAuthenticated)
                return defaultView;
            
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
            if (usuario != null)
            {
                switch (usuario.IdRol)
                {
                    case 1:
                    case 2:
                    case 3:
                        return RedirectToAction("Index", "Administrativo", new { area = "Usuario" });
                    case 4:         //Egresado
                        return RedirectToAction("Index", "Egresado", new { area = "Usuario" });
                    case 5:         //Empleador
                        return RedirectToAction("Index", "Empleador", new { area = "Usuario" });
                }
                
                // var query = from modulo in _context.Modulos
                //     join operacion in _context.Operaciones on modulo.Id equals operacion.IdModulo
                //     join rolOp in _context.RolOperacions on operacion.Id equals rolOp.IdOperacion into matches
                //     from match in matches.DefaultIfEmpty()
                //     where match.IdRol == usuario.IdRol
                //     select modulo;
                //
                // if (query.Any())
                // {
                //     HttpContext.Session.SetInt32("Module", query.First().Id);
                //     return RedirectToAction("Index", "Administrativo", new { area = "Usuario" });
                // }
            }

            return defaultView;
        }
    }
}