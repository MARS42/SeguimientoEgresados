using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace SeguimientoEgresados.Services;

public class CloudinaryService
{
    private readonly IOptions<CloudinaryOptions> _config;
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinaryOptions> config)
    {
        _config = config;
        Account cuenta = new (_config.Value.Server, _config.Value.ApiKey, _config.Value.ApiSecret);
        _cloudinary = new (cuenta);
    }
    
    public async Task SubirImagen()
    {
        Console.WriteLine(_config.Value.ApiKey);
        Console.WriteLine(_config.Value.ApiSecret);
        Console.WriteLine(_config.Value.Server);
        await Task.CompletedTask;
    }
}