using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SeguimientoEgresados.Models.ViewModels;

namespace SeguimientoEgresados.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly IOptions<CloudinaryOptions> _config;
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinaryOptions> config)
    {
        Console.WriteLine("config:" + config);
        _config = config;
        Account cuenta = new(_config.Value.Server, _config.Value.ApiKey, _config.Value.ApiSecret);
        _cloudinary = new(cuenta);
        _cloudinary.Api.Secure = true;
        Console.WriteLine("cloud:" + _cloudinary);
    }
    
    [Authorize(Roles = "Administrador,Moderador,Capturista")]
    public async Task<string[]> SubirImagen(IFormFile model, string folder = "default", string subfolder = "")
    {
        //Console.WriteLine("file: "+model.Imagen.ToString());
        try
        {
            //Console.WriteLine("this:"+this);
            //Console.WriteLine("file: "+ (model.Imagen == null ? "null" : model.Imagen.ToString()));
            //reads the Image in the IFormFile into a bytes then we convert    this to a base64 string
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await model.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);
     
     
            var prefix = @"data:image/png;base64,";
            var imagePath = prefix + base64;
     
            // create a new ImageUploadParams object and assign the directory name
            var uploadParams = new ImageUploadParams()
     
            {
                File = new FileDescription(imagePath),
                Folder = $"{folder}/{subfolder}"
            };
            
            var uploadParamsThumbnail = new ImageUploadParams()
     
            {
                File = new FileDescription(imagePath),
                Folder = $"{folder}/{subfolder}/Thumbnails",
                Transformation = new Transformation().AspectRatio("1.0").Width(64).Height(64).Crop("fill")
            };
            // pass the new ImageUploadParams object to the UploadAsync method of the Cloudinary Api
     
     
            var uploadResult = await _cloudinary.UploadAsync(@uploadParams);
            var uploadThumbnailResult = await _cloudinary.UploadAsync(@uploadParamsThumbnail);

            return new [] { uploadResult.SecureUrl.AbsoluteUri, uploadThumbnailResult.SecureUrl.AbsoluteUri };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    [Authorize(Roles = "Egresado,Empleador,Administrador,Moderador,Capturista")]
    public async Task<string> SubirImagenUsuario(IFormFile img, string usuario)
    {
        try
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await img.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);
     
     
            var prefix = @"data:image/png;base64,";
            var imagePath = prefix + base64;

            var uploadParams = new ImageUploadParams()
     
            {
                File = new FileDescription(imagePath),
                Folder = $"_ProfilePhotos/{usuario}"
            };
            
            var uploadResult = await _cloudinary.UploadAsync(@uploadParams);

            return uploadResult.SecureUrl.AbsoluteUri;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}