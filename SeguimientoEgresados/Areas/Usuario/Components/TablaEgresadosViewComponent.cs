using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;

namespace SeguimientoEgresados.Areas.Usuario.Components;

[ViewComponent]
public class TablaEgresadosViewComponent : ViewComponent
{
    private readonly SeguimientoEgresadosContext _db;

    public TablaEgresadosViewComponent(SeguimientoEgresadosContext context)
    {
        _db = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var query =
            from egresado in _db.Egresados
            join usuario in _db.Usuarios on egresado.IdUsuario equals usuario.Id into matches
            from match in matches.DefaultIfEmpty()
            select new EgresadoViewModel(match.Id, egresado.Id)
            {
                Nombre = match.Nombre,
                Ape1 = match.ApellidoPaterno,
                Ape2 = match.ApellidoMaterno,
                Correo = match.Email,
                NoControl = egresado.NControl,
                FechaRegistro = match.Fecha,
                Carrera = _db.Carreras.Where(c => c.Id.Equals(egresado.IdCarrera)).FirstOrDefault().Nombre
            };
    
        return View(await query.AsNoTracking().ToListAsync());
    }
}