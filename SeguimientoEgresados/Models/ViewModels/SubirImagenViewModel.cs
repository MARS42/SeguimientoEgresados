namespace SeguimientoEgresados.Models.ViewModels;

public class SubirImagenViewModel
{
    public string Titulo { get; set; }
    public IFormFile Imagen { get; set; }
    public string Descripcion { get; set; }
    public string Tags { get; set; }
}