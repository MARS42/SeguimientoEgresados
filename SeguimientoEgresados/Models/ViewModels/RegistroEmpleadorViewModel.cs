using System.ComponentModel.DataAnnotations;

namespace SeguimientoEgresados.Models.ViewModels;

public class RegistroEmpleadorViewModel
{
    [Required]
    public string Nombres { get; set; }
    [Required]
    public string Apellidos { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string NombreEmpresa { get; set; }
    [Required]
    public string RFC { get; set; }
    [Required]
    public string RazonSocial { get; set; }
    [Required]
    public string Domicilio { get; set; }
    [Required]
    public string Colonia { get; set; }
    [Required]
    public string CodigoPostal { get; set; }
    [Required]
    public string Pais { get; set; }
    [Required]
    public string Estado { get; set; }
    [Required]
    public string Municipio { get; set; }
    [Required]
    public string EmailEmpresa { get; set; }
    [Required]
    public string Telefono { get; set; }
    [Required]
    public string Website { get; set; }
}