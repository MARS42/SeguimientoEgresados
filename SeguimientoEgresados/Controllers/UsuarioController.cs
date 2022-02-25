using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Filters;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Controllers
{
    public class UsuarioController : Controller
    {
        private Usuario _usuario;
        private readonly SeguimientoEgresadosContext _context;

        public UsuarioController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            _usuario = HttpContext.Session.Get<Usuario>("User");
            
            if (_usuario == null)
                return RedirectToAction("Ingresar", "Inicio");
            
            return View(_usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Inicio");
        }
    }
}