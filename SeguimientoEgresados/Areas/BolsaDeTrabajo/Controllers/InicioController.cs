using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.BolsaDeTrabajo.Controllers
{
    [Area("BolsaDeTrabajo")]
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}