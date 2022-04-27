using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Filters;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Controllers;

public class InicioController : Controller
{
    private readonly ILogger<InicioController> _logger;
    private INotificacionesService _notificaciones;

    public InicioController(ILogger<InicioController> logger, INotificacionesService notificaciones)
    {
        _logger = logger;
        _notificaciones = notificaciones;
    }

    //[AutorizarUser(idOperacion:1)]
    public async Task<IActionResult> Index()
    {
        await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
        return View();
    }

    //[AutorizarUser(idOperacion:3)]
    public async Task<IActionResult> Privacy()
    {
        await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    [HttpGet]
    public IActionResult _RealizarReporte(string area)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
            return PartialView();
        
        bool hasReport = HttpContext.Session.Get<bool>($"reporte:{area}");
        
        return hasReport ? PartialView("_EsperaReporte") : PartialView();
    }

    public IActionResult _EnviarReporte(string titulo, string descripcion, string area)
    {
        if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descripcion) || string.IsNullOrEmpty(area) || HttpContext.Session.Get<bool>($"reporte:{area}"))
            return Ok();
        
        Console.WriteLine(titulo);
        Console.WriteLine(descripcion);
        Console.WriteLine(area);
        
        if (User.Identity == null || !User.Identity.IsAuthenticated)
            HttpContext.Session.Set($"reporte:{area}", true);
        
        return Ok();
    }
}