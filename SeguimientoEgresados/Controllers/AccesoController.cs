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

        public IActionResult Index()
        {
            if (HttpContext.Session.Get<Usuario>("User") != null)
                return RedirectToAction("Index", "Perfil");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string User, string Pass)
        {
            try
            {
                var oUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email.Equals(User.Trim()) && u.Password.Equals(Pass.Trim()));
                if (oUser == null)
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                    return View();
                }

                //Session["User"] = oUser;
                HttpContext.Session.Set<Usuario>("User", oUser);

                return RedirectToAction("Index", "Inicio");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }
    }
}