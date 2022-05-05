using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class IntervalosCuestionariosViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Escribe un número entero")]
    public int MesesEgresado { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Escribe un número entero")]
    public int MesesEmpleador { get; set; }
}