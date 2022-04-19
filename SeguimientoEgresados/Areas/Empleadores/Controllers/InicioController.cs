using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;

namespace SeguimientoEgresados.Areas.Empleadores.Controllers
{
    [Area("Empleadores")]
    public class InicioController : Controller
    {
        private readonly SeguimientoEgresadosContext _db;
        private readonly INotificacionesService _notificaciones;
        
        public InicioController(SeguimientoEgresadosContext db, INotificacionesService notificaciones)
        {
            _db = db;
            _notificaciones = notificaciones;
        }
        
        // [HttpGet, Route("~/Empleadores/")]
        public async Task<IActionResult> Index()
        {
            var query =
                from empresa in _db.Empresas
                join usuario in _db.Usuarios on empresa.IdUsuario equals usuario.Id into empresasUsuario
                from empresaUsuario in empresasUsuario.DefaultIfEmpty()
                select new VisitaEmpresaViewModel(empresaUsuario.Id, empresa.Id)
                {
                    Nombre = empresa.Nombre,
                    Descripcion = empresa.Descripcion,
                    Correo = empresa.CorreoEmpresa,
                    Telefono = empresa.Telefono,
                    Website = empresa.Website,
                    Rfc = empresa.Rfc,
                    FechaRegistro = empresaUsuario.Fecha,
                    NombreRep = empresaUsuario.Nombre,
                    Ape1Rep = empresaUsuario.ApellidoPaterno,
                    Ape2Rep = empresaUsuario.ApellidoMaterno,
                    LogoImagen = empresaUsuario.UrlImg,
                    Convenio = empresa.IdConvenio != null
                };

            query = query.OrderBy(e => e.Nombre);
            
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
            return View(await query.AsNoTracking().ToListAsync());
        }
    }
}