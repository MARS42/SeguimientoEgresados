namespace SeguimientoEgresados.Models.ViewModels;

public class VerVacanteViewModel
{
    public int Id { get; set; }
    
    public int IdEmpresa { get; set; }
    
    public string Titulo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Funciones { get; set; } = null!;
    public string Requisitos { get; set; } = null!;
    public string TipoContrato { get; set; } = null!;
    public string Modalidad { get; set; } = null!;
    public string Horario { get; set; } = null!;
    public string Ofertas { get; set; } = null!;
    public DateTime? Fecha { get; set; }
    
    public string NombreEmpresa { get; set; } = null!;
    public string? LogoEmpresa { get; set; }

    public bool Convenio { get; set; }
}