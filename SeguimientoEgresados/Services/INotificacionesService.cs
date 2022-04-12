using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SeguimientoEgresados.Models.ViewModels;

namespace SeguimientoEgresados.Services;

public interface INotificacionesService
{
    public Task<AvisoCuestionario> VerificarCuestionario(HttpContext httpContext, ViewDataDictionary viewData, bool forceShow);
}