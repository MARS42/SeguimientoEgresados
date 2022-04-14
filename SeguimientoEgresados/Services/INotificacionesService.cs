using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SeguimientoEgresados.Models.ViewModels;

namespace SeguimientoEgresados.Services;

public interface INotificacionesService
{
    public Task<AvisoCuestionario> VerificarCuestionario(ClaimsPrincipal claimsPrincipal, HttpContext httpContext, ViewDataDictionary viewData, bool forceShow);
}