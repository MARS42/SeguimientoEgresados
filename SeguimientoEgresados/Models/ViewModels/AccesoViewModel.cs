using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class AccesoViewModel
{
    [EmailAddress(ErrorMessage = "Es necesario el correo electrónico")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Es necesario la contraseña")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
}