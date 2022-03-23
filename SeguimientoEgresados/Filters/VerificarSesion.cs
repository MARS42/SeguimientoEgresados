using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeguimientoEgresados.Areas.Usuario.Controllers;
using SeguimientoEgresados.Controllers;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Filters;

public class VerificarSesion : ActionFilterAttribute
{
    private Usuario oUsuario;
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        try
        {
            base.OnActionExecuting(filterContext);

            //oUsuario = (Usuario)HttpContext.Current.Session["User"];
            oUsuario = filterContext.HttpContext.Session.Get<Usuario>("User");
            if (oUsuario == null)
            {
                //if (filterContext.Controller is AccesoController == false)
                //bool condition = filterContext.Controller is AccesoController == false && filterContext.Controller is InicioController == false;
                bool condition = filterContext.Controller is EmpleadorController or EgresadoController;
                if (condition)
                {
                    filterContext.HttpContext.Response.Redirect("/Acceso/");
                }
            }

        }
        catch (Exception)
        {
            filterContext.Result = new RedirectResult("~/Acceso/");
        }

    }
}