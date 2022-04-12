using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Filters;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Services;

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
        await _notificaciones.VerificarCuestionario(HttpContext, ViewData, false);
        return View();
    }

    //[AutorizarUser(idOperacion:3)]
    public async Task<IActionResult> Privacy()
    {
        await _notificaciones.VerificarCuestionario(HttpContext, ViewData, false);
        return View();
    }

    public IActionResult Ingresar()
    {
        //return RedirectToAction("Login", "Acceso");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}