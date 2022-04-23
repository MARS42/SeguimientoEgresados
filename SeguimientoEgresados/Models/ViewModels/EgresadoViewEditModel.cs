namespace SeguimientoEgresados.Models.ViewModels;

public class EgresadoViewEditModel : Egresado
{
    public int Id { get; set; }
    public string Telefono { get; set; } = null!;
    public DateTime? FechaNacimiento { get; set; }
    public string PaisNacimiento { get; set; } = null!;
    public string EstadoNacimiento { get; set; } = null!;
    public string MunicipioNacimiento { get; set; } = null!;
    public string Colonia { get; set; } = null!;
    public int Cp { get; set; }
    public string Domicilio { get; set; } = null!;
    public string Pais { get; set; } = null!;
    public string Municipio { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string NControl { get; set; } = null!;
    public DateTime? FechaEgreso { get; set; }
    public int IdGenero { get; set; }
    public int IdEstadoCivil { get; set; }
    public int IdCarrera { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FechaInicio { get; set; }
}