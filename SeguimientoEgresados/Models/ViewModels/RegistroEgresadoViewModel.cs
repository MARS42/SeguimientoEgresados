using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class RegistroEgresadoViewModel
{
    [Required] public string Nombres { get; set; }
    [Required] public string ApellidoPaterno { get; set; }
    [Required] public string ApellidoMaterno { get; set; }
    [Required] [DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico no valido")] public string Email { get; set; }
    [Required] public string Password { get; set; }
    
    [Required] [DataType(DataType.PhoneNumber)]public string Telefono { get; set; }
    [Required] public int idGenero { get; set; }
    [Required] public int idEstadoCivil { get; set; }
    
    [Required] [DataType(DataType.Date)]public DateTime FechaNacimiento { get; set; }
    [Required] public string PaisNacimiento { get; set; }
    [Required] public string EstadoNacimiento { get; set; }
    [Required] public string MunicipioNacimiento { get; set; }

    [Required] public string Domicilio { get; set; }
    [Required] public string Colonia { get; set; }
    [Required] [DataType(DataType.PostalCode)] public string CodigoPostal { get; set; }
    [Required] public string Pais { get; set; }
    [Required] public string Estado { get; set; }
    [Required] public string Municipio { get; set; }
    
    [Required] public string NoControl { get; set; }
    [Required] public int idCarrera { get; set; }
    [Required][DataType(DataType.Date)] public DateTime FechaEgreso { get; set; }
}