using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SeguimientoEgresados.Filters;
using SeguimientoEgresados.Models;

namespace SeguimientoEgresados.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AutorizarUser(idOperacion:1)]
    public IActionResult Index()
    {
        return View();
    }

    [AutorizarUser(idOperacion:3)]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}