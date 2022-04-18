using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;

namespace SeguimientoEgresados.Areas.Galeria.Controllers
{
    [Area("Galeria")]
    public class InicioController : Controller
    {
        private readonly SeguimientoEgresadosContext _db;
        private readonly INotificacionesService _notificaciones;
        
        public InicioController(SeguimientoEgresadosContext db, INotificacionesService notificaciones)
        {
            _db = db;
            _notificaciones = notificaciones;
        }
        
        public async Task<IActionResult> Index()
        {
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);

            var query = from galeria in _db.Galeria select new VerAlbumViewModel()
            {
                Id = galeria.Id,
                Descripcion = galeria.Descripcion,
                Nombre = galeria.Nombre,
                Fecha = galeria.Fecha
            };
            
            List<VerAlbumViewModel> albumes = await query.ToListAsync();
            for (int i = 0; i < albumes.Count; i++)
            {
                ImagenGalerium algunaImagen = await _db.ImagenGaleria.FirstOrDefaultAsync(img => img.IdAlbum.Equals(albumes[i].Id));
                albumes[i].MiniaturaUrl = algunaImagen.Url;

            }
            
            return View(albumes);
        }

        public async Task<IActionResult> _VerAlbum(int id)
        {
            var album = await _db.Galeria.FirstOrDefaultAsync(g => g.Id.Equals(id));
            return View(album);
        }

        public async Task<IActionResult> GetImagenes(int idGaleria)
        {
            var query = from img in _db.ImagenGaleria where img.IdAlbum.Equals(idGaleria) select img;

            return Json(await query.AsNoTracking().ToListAsync());
        }
    }
}