using SeguimientoEgresados.Models.ViewModels;

namespace SeguimientoEgresados.Services;

public interface INotificacionesService
{
    public Task<AvisoCuestionario> VerificarCuestionario(HttpContext httpContext);
}