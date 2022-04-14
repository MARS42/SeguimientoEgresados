using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SeguimientoEgresados.Models;
using SeguimientoEgresados.Models.ViewModels;
using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Services;

public class NotificacionesService : INotificacionesService
{
    private readonly SeguimientoEgresadosContext _context;

    public NotificacionesService(SeguimientoEgresadosContext context)
    {
        _context = context;
    }
    
    public async Task<AvisoCuestionario> VerificarCuestionario(ClaimsPrincipal claimsPrincipal, HttpContext httpContext, ViewDataDictionary viewData, bool forceShow)
    {
        //Usuario user = httpContext.Session.Get<Usuario>("User")!;

        if (!claimsPrincipal.Identity.IsAuthenticated)
            return null;
        
        var email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
        Cuestionario? cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(usuario.Id));
        
        if (cuestionario == null)
        {
            viewData["Aviso"] = new AvisoCuestionario("Bienvenido al seguimiento de egresados",
                "El sistema de seguimiento egresados centra y ofrece su información para el conocimiento y beneficio de todos.<br/>Esto es posible mediante la obtención de información de los miembros que la integran.Por ello, debes realizar un cuestionario inicial y completar tu registro.", forceShow);
            return null;
        }

        if (DateTime.Compare(cuestionario.ProximaAplicacion, DateTime.Now) <= 0)
        {
            viewData["Aviso"] = new AvisoCuestionario("Actualización de cuestionario",
                $"Tu última aplicación del cuestinario fue el {cuestionario.UltimaAplicacion.Value.ToString()}, puedes volver a aplicarlo para mantener tus datos actualizados.", forceShow);
            return null;
        }

        return null;
    }
}