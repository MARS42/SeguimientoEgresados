using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.Egresados.Controllers
{
    [Area("Egresados")]
    public class InicioController : Controller
    {
        [HttpGet, Route("~/Egresados/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}