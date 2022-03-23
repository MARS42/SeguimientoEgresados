using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Areas.About.Controllers
{
    [Area("About")]
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}