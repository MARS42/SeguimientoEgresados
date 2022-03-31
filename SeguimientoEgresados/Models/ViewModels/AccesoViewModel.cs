using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class AccesoViewModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}