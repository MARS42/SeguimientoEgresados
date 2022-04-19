using Microsoft.AspNetCore.Authorization;

namespace SeguimientoEgresados.Services;

public interface ICloudinaryService
{
    [Authorize(Roles = "Administrador,Moderador,Capturista")]
    public Task<string[]> SubirImagen(IFormFile model, string folder = "default", string subfolder = "");

    [Authorize(Roles = "Egresado,Empleador,Administrador,Moderador,Capturista")]
    public Task<string> SubirImagenUsuario(IFormFile img, string usuario);
}