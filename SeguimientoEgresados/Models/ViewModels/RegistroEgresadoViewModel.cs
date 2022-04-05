using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class RegistroEgresadoViewModel
{
    [Required] public string Nombres { get; set; }
    [Required] public string Apellidos { get; set; }
    [Required] [DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico no valido")] public string Email { get; set; }
    [Required] public string Password { get; set; }
    
    [Required] [DataType(DataType.PhoneNumber)]public string Telefono { get; set; }
    [Required] public string Genero { get; set; }
    [Required] public string EstadoCivil { get; set; }
    
    [Required] [DataType(DataType.Date)]public DateTime FechaNacimiento { get; set; }
    [Required] public string PaisNacimiento { get; set; }
    [Required] public string EstadoNacimiento { get; set; }
    [Required] public string MunicipioNacimiento { get; set; }

    [Required] public string Domicilio { get; set; }
    [Required] public string Colonia { get; set; }
    [Required] [DataType(DataType.PostalCode)] public string CodigoPostal { get; set; }
    [Required] public string Pais { get; set; }
    [Required] public string Estado { get; set; }
    [Required] public string Municipo { get; set; }
    
    [Required] public string NoControl { get; set; }
    [Required] public string Carrera { get; set; }
    [Required] public DateTime FechaEgreso { get; set; }
}