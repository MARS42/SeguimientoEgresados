namespace SeguimientoEgresados.Models.ViewModels;

public class VerAlbumViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime? Fecha { get; set; }
    
    public string MiniaturaUrl { get; set; }
}