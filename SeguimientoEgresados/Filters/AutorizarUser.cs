using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
public class AutorizarUser : Attribute, IAuthorizationFilter
{
    private Usuario oUsuario;
    private SeguimientoEgresadosContext db;
    private int idOperacion;
    
    public AutorizarUser(int idOperacion = 0)
    {
        this.idOperacion = idOperacion;
    }

    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        String nombreOperacion = "";
        String nombreModulo = "";
        try
        {
            oUsuario = context.HttpContext.Session.Get<Usuario>("User");
            db = new SeguimientoEgresadosContext();
            var lstMisOperaciones = from m in db.RolOperacions
                where m.IdRol == oUsuario.IdRol
                      && m.IdOperacion == idOperacion
                select m;

            if (lstMisOperaciones.ToList().Count() ==0)
            {
                var oOperacion = db.Operaciones.Find(idOperacion);
                int? idModulo =oOperacion.IdModulo;
                nombreOperacion = getNombreDeOperacion(idOperacion);
                nombreModulo = getNombreDelModulo(idModulo);
                context.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "&modulo=" + nombreModulo + "&msjeErrorExcepcion=");
                Console.Write("Sinpermiso");
            }
            Console.Write("Con permiso");
        }
        catch (Exception ex)
        {
            context.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "&modulo=" + nombreModulo + "&msjeErrorExcepcion=" + ex.Message);
        }
    }
    
    public string getNombreDeOperacion(int idOperacion)
    {
        var ope = from op in db.Operaciones
            where op.Id == idOperacion
            select op.Nombe;
        String nombreOperacion;
        try
        {
            nombreOperacion = ope.First();
        }
        catch (Exception)
        {
            nombreOperacion = "";
        }
        return nombreOperacion;
    }

    public string getNombreDelModulo(int? idModulo)
    {
        var modulo = from m in db.Modulos
            where m.Id == idModulo
            select m.Nombre;
        String nombreModulo;
        try
        {
            nombreModulo = modulo.First();
        }
        catch (Exception)
        {
            nombreModulo = "";
        }
        return nombreModulo;
    }
}