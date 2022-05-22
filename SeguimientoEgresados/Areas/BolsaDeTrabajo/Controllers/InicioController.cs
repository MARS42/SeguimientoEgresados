using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Services;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Areas.BolsaDeTrabajo.Controllers
{
    [Area("BolsaDeTrabajo")]
    public class InicioController : Controller
    {
        private readonly SeguimientoEgresadosContext _context;
        private readonly INotificacionesService _notificaciones;
        private readonly ICloudinaryService _cloudinary;
        
        public InicioController(SeguimientoEgresadosContext context, INotificacionesService notificaciones, ICloudinaryService cloudinary)
        {
            _context = context;
            _notificaciones = notificaciones;
            _cloudinary = cloudinary;
        }
        
        public async Task<IActionResult> Index()
        {
            await _notificaciones.VerificarCuestionario(User, HttpContext, ViewData, false);
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetVacantes(string ordenTabla, string busqueda, string filtroActual, int? pagina)
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
                //busqueda = filtroActual;
            }
            
            ViewData["Busqueda"] = busqueda;

            var query =
                    from vacante in _context.Vacantes
                    join empresa in _context.Empresas on vacante.IdEmpresa equals empresa.Id into vacantesEmpresa
                    from vacanteEmpresa in vacantesEmpresa.DefaultIfEmpty()
                    join usuario in _context.Usuarios on vacanteEmpresa.IdUsuario equals usuario.Id into usuariosVacanteEmpresa
                    from usuarioVacanteEmpresa in usuariosVacanteEmpresa.DefaultIfEmpty()
                    where usuarioVacanteEmpresa.Verificado != null && usuarioVacanteEmpresa.Verificado.Equals("true")
                    select new VerVacanteViewModel()
                    {
                        NombreEmpresa = vacanteEmpresa.Nombre,
                        Titulo = vacante.Titulo,
                        Descripcion = vacante.Descripcion,
                        Funciones = vacante.Funciones,
                        Requisitos = vacante.Requisitos,
                        Ofertas = vacante.Ofertas,
                        TipoContrato = vacante.TipoContrato,
                        Modalidad = vacante.Modalidad,
                        Horario = vacante.Horario,
                        Fecha = vacante.Fecha,
                        Id = vacante.Id,
                        LogoEmpresa = usuarioVacanteEmpresa.UrlImg,
                        Convenio = vacanteEmpresa.IdConvenio != null
                    };

            // var query =
            //     from vacante in _context.Vacantes
            //     join empresa in _context.Empresas on vacante.IdEmpresa equals empresa.Id into vacantesEmpresa
            //     from vacanteEmpresa in vacantesEmpresa.DefaultIfEmpty()
            //     select new VerVacanteViewModel()
            //     {
            //         NombreEmpresa = vacanteEmpresa.Nombre,
            //         Titulo = vacante.Titulo,
            //         Descripcion = vacante.Descripcion,
            //         Funciones = vacante.Funciones,
            //         Requisitos = vacante.Requisitos,
            //         Ofertas = vacante.Ofertas,
            //         TipoContrato = vacante.TipoContrato,
            //         Modalidad = vacante.Modalidad,
            //         Horario = vacante.Horario,
            //         Fecha = vacante.Fecha,
            //         Id = vacante.Id,
            //         LogoEmpresa = _context.Usuarios.FirstOrDefault(u => u.Id.Equals(vacanteEmpresa.IdUsuario))?.UrlImg
            //     };
            
            
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(v => v.NombreEmpresa.Contains(busqueda)
                                         || v.Titulo.Contains(busqueda)
                                         || v.Descripcion.Contains(busqueda)
                                         || v.Modalidad.Contains(busqueda)
                                         || v.TipoContrato.Contains(busqueda)
                                         || v.Ofertas.Contains(busqueda));
            }

            switch (ordenTabla)
            {
                case "nombre_desc":
                    query = query.OrderByDescending(e => e.Titulo);
                    break;
                case "fecha_desc":
                    query = query.OrderByDescending(e => e.Fecha);
                    break;
                case "fecha_asc":
                    query = query.OrderBy(e => e.Fecha);
                    break;
                case "rep_desc":
                    query = query.OrderByDescending(e => e.NombreEmpresa);
                    break;
                case "rep_asc":
                    query = query.OrderBy(e => e.NombreEmpresa);
                    break;
                default:
                    query = query.OrderBy(e => e.Titulo);
                    break;
            }

            int pageSize = 10;
            return PartialView("_GetVacantes",  await ListaPaginada<VerVacanteViewModel>.CreateAsync(query.AsNoTracking(), pagina ?? 1, pageSize));
        }

        public async Task<IActionResult> VerDetallesVacante(int id)
        {
            var query =
                from vacante in _context.Vacantes
                join empresa in _context.Empresas on vacante.IdEmpresa equals empresa.Id into vacantesEmpresa
                from vacanteEmpresa in vacantesEmpresa.DefaultIfEmpty()
                where  vacante.Id.Equals(id)
                select new VerVacanteViewModel()
                {
                    NombreEmpresa = vacanteEmpresa.Nombre,
                    Titulo = vacante.Titulo,
                    Descripcion = vacante.Descripcion,
                    Funciones = vacante.Funciones,
                    Requisitos = vacante.Requisitos,
                    Ofertas = vacante.Ofertas,
                    TipoContrato = vacante.TipoContrato,
                    Modalidad = vacante.Modalidad,
                    Horario = vacante.Horario,
                    Fecha = vacante.Fecha,
                    Id = vacante.Id,
                    //LogoEmpresa = usuarioVacanteEmpresa.UrlImg,
                    Convenio = vacanteEmpresa.IdConvenio != null
                };
            return PartialView("_DetallesVacante", await query.FirstOrDefaultAsync());
        }
        
        public async Task<IActionResult> Postula(int id){
            var query =
                from vacante in _context.Vacantes
                join empresa in _context.Empresas on vacante.IdEmpresa equals empresa.Id into vacantesEmpresa
                from vacanteEmpresa in vacantesEmpresa.DefaultIfEmpty()
                where  vacante.Id.Equals(id)
                select new VerVacanteViewModel()
                {
                    NombreEmpresa = vacanteEmpresa.Nombre,
                    Titulo = vacante.Titulo,
                    Descripcion = vacante.Descripcion,
                    Funciones = vacante.Funciones,
                    Requisitos = vacante.Requisitos,
                    Ofertas = vacante.Ofertas,
                    TipoContrato = vacante.TipoContrato,
                    Modalidad = vacante.Modalidad,
                    Horario = vacante.Horario,
                    Fecha = vacante.Fecha,
                    Id = vacante.Id,
                    IdEmpresa = vacante.IdEmpresa,
                    //LogoEmpresa = usuarioVacanteEmpresa.UrlImg,
                    Convenio = vacanteEmpresa.IdConvenio != null
                };
            return View(await query.FirstOrDefaultAsync());
        }

        [Authorize(Roles = "Egresado")]
        public async Task<IActionResult> Postularse(string idVacante, string idEmpresa, IFormFile? cvInput)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            Models.Usuario? user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
            Egresado egresado = await _context.Egresados.FirstOrDefaultAsync(e => e.IdUsuario.Equals(user.Id));

            int idE = int.Parse(idEmpresa);
            Empresa? empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Id.Equals(idE));
            
            if (user == null)
                return NotFound();

            string cv_url = "No URL";
            if (cvInput != null)
            {
                cv_url = await _cloudinary.SubirCv(cvInput, empresa.Nombre, egresado.NControl);
            }

            Postulante postulante = new Postulante()
            {
                Fecha = DateTime.Now,
                CvUrl = cv_url,
                IdEgresado = egresado.Id,
                IdVacante = int.Parse(idVacante)
            };

            _context.Postulantes.Add(postulante);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"File: {cvInput.Name}");
            
            return Ok();
        }
    }
}