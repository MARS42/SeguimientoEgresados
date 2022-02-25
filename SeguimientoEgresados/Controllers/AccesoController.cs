using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
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
    
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string User, string Pass)
        {
            try
            {
                // var oUser = (from d in _context.Usuarios
                //     where d.Email == User.Trim() && d.Password == Pass.Trim()
                //     select d).FirstOrDefault();

                var oUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email.Equals(User.Trim()) && u.Password.Equals(Pass.Trim()));
                if (oUser == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View();
                }

                //Session["User"] = oUser;
                HttpContext.Session.Set<Usuario>("User", oUser);

                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }
    }
}