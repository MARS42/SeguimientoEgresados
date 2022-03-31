using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeguimientoEgresados.Controllers
{
    public class RegistroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string Tipo){
            
            if(Tipo.Equals("Empleador") || Tipo.Equals("Egresado"))
                return RedirectToAction(Tipo.Equals("Egresado") ? "Egresado" : "Empleador");

            return NotFound();
        }
        
        public IActionResult Egresado()
        {
            return View();
        }

        public IActionResult Empleador()
        {
            return View();
        }
    }
}