using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Egresados.Controllers
{
    [Area("Egresados")]
    public class AccesoController : Controller
    {
        private Usuario _usuario;
        private readonly SeguimientoEgresadosContext _context;

        public AccesoController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
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

                return RedirectToAction("Index", "Inicio", new { area= "Egresados" });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            return View();
        }
    }
}