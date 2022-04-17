namespace SeguimientoEgresados.Services;

public interface ICloudinaryService
{
    public Task<string> SubirImagen(IFormFile model, string folder = "default", string subfolder = "");
}