using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.Empleadores.Controllers
{
    [Area("Empleadores")]
    public class InicioController : Controller
    {
        [HttpGet, Route("~/Empleadores/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}