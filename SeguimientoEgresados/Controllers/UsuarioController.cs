using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Filters;

namespace SeguimientoEgresados.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CerrarSesion()
        {
            return Ok();
        }
    }
}