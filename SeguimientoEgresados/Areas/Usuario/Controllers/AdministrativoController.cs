using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.Usuario.Controllers
{
    [Authorize(Roles = "Administrador,Moderador,Capturista")]
    [Area("Usuario")]
    public class AdministrativoController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;

        public AdministrativoController(SeguimientoEgresadosContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            //Models.Usuario? user = HttpContext.Session.Get<Models.Usuario>("User");
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
            ViewData["SidebarItem"] = 1;
            return View(usuario);
        }

        public async Task<IActionResult> General()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Egresados()
        {
            ViewData["SidebarItem"] = 2;
            return PartialView();
        }
        
        public async Task<IActionResult> Empleadores()
        {
            ViewData["SidebarItem"] = 3;
            return PartialView(await _context.Empresas.ToListAsync());
        }
        
        public async Task<IActionResult> CambiosPassword()
        {
            ViewData["SidebarItem"] = 4;
            return PartialView();
        }
        
        public async Task<IActionResult> Reportes()
        {
            ViewData["SidebarItem"] = 5;
            return PartialView();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            //HttpContext.Session.Remove("User");
            //HttpContext.Session.Remove("Module");
            return RedirectToAction("Index", "Inicio", new { area="" });
        }

        public async Task<IActionResult> GetEgresados(string ordenTabla, string busqueda, string filtroActual, int? pagina)
        {
            ViewData["OrdenActual"] = ordenTabla;
            ViewData["NombreSort"] = String.IsNullOrEmpty(ordenTabla) ? "nombre_desc" : "";
            ViewData["RepSort"] = ordenTabla == "rep_desc" ? "rep_asc" : "fecha_desc";
            ViewData["RegSort"] = ordenTabla == "fecha_desc" ? "fecha_asc" : "fecha_desc";
            
            if (busqueda != null)
            {
                pagina = 1;
            }
            else
            {
                busqueda = filtroActual;
            }
            
            ViewData["Busqueda"] = busqueda;
            
            var query =
                from egresado in _context.Egresados
                join usuario in _context.Usuarios on egresado.IdUsuario equals usuario.Id into matches
                from match in matches.DefaultIfEmpty()
                select new EgresadoViewModel(match.Id, egresado.Id)
                {
                    Nombre = match.Nombre,
                    Ape1 = match.ApellidoPaterno,
                    Ape2 = match.ApellidoMaterno,
                    Correo = match.Email,
                    NoControl = egresado.NControl,
                    FechaRegistro = match.Fecha,
                    Carrera = _context.Carreras.Where(c => c.Id.Equals(egresado.IdCarrera)).FirstOrDefault().Nombre
                };
            
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(e => e.Nombre.Contains(busqueda)
                                         || e.Ape1.Contains(busqueda)
                                         || e.Ape2.Contains(busqueda)
                                         || e.Correo.Contains(busqueda)
                                         || e.Carrera.Contains(busqueda));
            }
            
            int pageSize = 1;
            return PartialView("_GetEgresados",  await ListaPaginada<EgresadoViewModel>.CreateAsync(query.AsNoTracking(), pagina ?? 1, pageSize));
        }

        public async Task<IActionResult> GetEmpleadores(string ordenTabla, string busqueda, string filtroActual, int? pagina)
        {
            ViewData["OrdenActual"] = ordenTabla;
            ViewData["NombreSort"] = String.IsNullOrEmpty(ordenTabla) ? "nombre_desc" : "";
            ViewData["RepSort"] = ordenTabla == "rep_desc" ? "rep_asc" : "fecha_desc";
            ViewData["RegSort"] = ordenTabla == "fecha_desc" ? "fecha_asc" : "fecha_desc";
            
            if (busqueda != null)
            {
                pagina = 1;
            }
            else
            {
                busqueda = filtroActual;
            }
            
            ViewData["Busqueda"] = busqueda;
            
            var query =
                from empresa in _context.Empresas
                join usuario in _context.Usuarios on empresa.IdUsuario equals usuario.Id into matches
                from match in matches.DefaultIfEmpty()
                select new EmpresaViewModel(match.Id, empresa.Id)
                {
                    Nombre = empresa.Nombre,
                    Correo = empresa.CorreoEmpresa,
                    Rfc = empresa.Rfc,
                    Telefono = empresa.Telefono,
                    NombreRep = match.Nombre,
                    Ape1Rep = match.ApellidoPaterno,
                    Ape2Rep = match.ApellidoMaterno,
                    FechaRegistro = match.Fecha,
                    
                };
            
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(e => e.Nombre.Contains(busqueda)
                                         || e.NombreRep.Contains(busqueda)
                                         || e.Ape1Rep.Contains(busqueda)
                                         || e.Ape2Rep.Contains(busqueda)
                                         || e.Rfc.Contains(busqueda));
            }

            switch (ordenTabla)
            {
                case "nombre_desc":
                    query = query.OrderByDescending(e => e.Nombre);
                    break;
                case "fecha_desc":
                    query = query.OrderByDescending(e => e.FechaRegistro);
                    break;
                case "fecha_asc":
                    query = query.OrderBy(e => e.FechaRegistro);
                    break;
                case "rep_desc":
                    query = query.OrderByDescending(e => e.Representante);
                    break;
                case "rep_asc":
                    query = query.OrderBy(e => e.Representante);
                    break;
                default:
                    query = query.OrderBy(e => e.Nombre);
                    break;
            }
            
            int pageSize = 3;
            return PartialView("_GetEmpleadores",  await ListaPaginada<EmpresaViewModel>.CreateAsync(query.AsNoTracking(), pagina ?? 1, pageSize));
        }

        public async Task<IActionResult> BusquedaEmpleadores(string term)
        {
            var query = from empresa in _context.Empresas
                where empresa.Nombre.Contains(term)
                select empresa.Nombre;

            return Json(await query.ToListAsync());
        }
    }
}