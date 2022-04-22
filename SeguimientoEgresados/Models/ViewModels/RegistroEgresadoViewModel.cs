using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class RegistroEgresadoViewModel
{
    [Required (ErrorMessage = "Nombre obligatorio")] public string Nombres { get; set; }
    [Required (ErrorMessage = "Apellido requerido")] public string ApellidoPaterno { get; set; }
    [Required (ErrorMessage = "Apellido requerido")] public string ApellidoMaterno { get; set; }
    [Required (ErrorMessage = "Email requerido")] [DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico no valido")] public string Email { get; set; }
    
    [Required(ErrorMessage = "Es requerida una contraseña")]
    [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }
    
    [Required (ErrorMessage = "Teléfono requerido")] [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono no válido")]public string Telefono { get; set; }
    [Required] public int idGenero { get; set; }
    [Required] public int idEstadoCivil { get; set; }
    
    [Required (ErrorMessage = "Obligatorio")] [DataType(DataType.Date)]public DateTime FechaNacimiento { get; set; }
    [Required (ErrorMessage = "Obligatorio")] public string PaisNacimiento { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string EstadoNacimiento { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string MunicipioNacimiento { get; set; }

    [Required(ErrorMessage = "Obligatorio")] public string Domicilio { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string Colonia { get; set; }
    [Required(ErrorMessage = "Obligatorio")] [DataType(DataType.PostalCode, ErrorMessage = "Código postal no válido")] public string CodigoPostal { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string Pais { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string Estado { get; set; }
    [Required(ErrorMessage = "Obligatorio")] public string Municipio { get; set; }
    
    [Required(ErrorMessage = "Número de control requerido")] public string NoControl { get; set; }
    [Required] public int idCarrera { get; set; }
    [Required (ErrorMessage = "Fecha de egreso obligatoria")][DataType(DataType.Date, ErrorMessage = "Formato de fecha no válida")] public DateTime FechaEgreso { get; set; }
}