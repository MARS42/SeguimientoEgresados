using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class CambioPasswordViewModel
{
    [Required (ErrorMessage = "Email requerido")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico no valido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Es necesario escribir la contraseña actual")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña actual")]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "Es requerida una contraseña")]
    [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string NewPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar nueva contraseña")]
    [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmNewPassword { get; set; }
}