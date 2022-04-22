using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class RegistroEmpleadorViewModel
{
    [Required(ErrorMessage = "Nombre requerido")]
    public string Nombres { get; set; }
    
    [Required (ErrorMessage = "Apellido requerido")]
    public string ApellidoPaterno { get; set; }
    
    [Required (ErrorMessage = "Apellido requerido")]
    public string ApellidoMaterno { get; set; }
    
    [Required(ErrorMessage = "Email requerido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Es requerida una contraseña")]
    [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }
    
    [Required (ErrorMessage = "Nombre de empresa requerido")]
    public string NombreEmpresa { get; set; }
    
    [Required (ErrorMessage = "Describa su empresa")]
    public string Descripcion { get; set; }
    
    [Required (ErrorMessage = "RFC requerido")]
    public string RFC { get; set; }
    
    [Required(ErrorMessage = "Escriba la razón social")]
    public string RazonSocial { get; set; }
    [Required(ErrorMessage = "Domicilio requerido")]
    public string Domicilio { get; set; }
    [Required (ErrorMessage = "Escriba la colonia")]
    public string Colonia { get; set; }
    [Required (ErrorMessage = "Código postal requerido")]
    public string CodigoPostal { get; set; }
    [Required (ErrorMessage = "Especifíque el país")]
    public string Pais { get; set; }
    [Required (ErrorMessage = "Escribe el estado")]
    public string Estado { get; set; }
    [Required (ErrorMessage = "Municipio requerido")]
    public string Municipio { get; set; }
    [Required (ErrorMessage = "Email de empresa requerido")]
    public string EmailEmpresa { get; set; }
    [Required (ErrorMessage = "Teléfono requerido")]
    public string Telefono { get; set; }
    
    [Required(AllowEmptyStrings = true)]
    public string Website { get; set; }
}