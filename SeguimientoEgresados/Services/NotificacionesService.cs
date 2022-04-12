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
    
    public async Task<AvisoCuestionario> VerificarCuestionario(HttpContext httpContext)
    {
        Usuario user = httpContext.Session.Get<Usuario>("User")!;

        if (user == null)
            return null;
        
        Cuestionario? cuestionario = await _context.Cuestionarios.FirstOrDefaultAsync(c => c.IdUsuario.Equals(user.Id));
        if(cuestionario == null)
            return new AvisoCuestionario("Bienvenido al seguimiento de egresados", "El sistema de seguimiento egresados centra y ofrece su información para el conocimiento y beneficio de todos.<br/>Esto es posible mediante la obtención de información de los miembros que la integran.Por ello, debes realizar un cuestionario inicial y completar tu registro.");

        if (DateTime.Compare(cuestionario.ProximaAplicacion, DateTime.Now) <= 0)
            return new AvisoCuestionario("Actualización de cuestionario", $"Tu última aplicación del cuestinario fue el {cuestionario.UltimaAplicacion.Value.ToString()}, puedes volver a aplicarlo para mantener tus datos actualizados.");

        return null;
    }
}