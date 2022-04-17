using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class SubirImagenViewModel
{
    [Required] public string Titulo { get; set; }

    [Required]
    public string Descripcion { get; set; }
    
    
    public string Tags { get; set; }
}