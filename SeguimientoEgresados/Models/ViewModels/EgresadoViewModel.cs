namespace SeguimientoEgresados.Models.ViewModels;

public class EgresadoViewModel
{
    public readonly int idUsuario, idEgresado;
    
    public string Nombre { get; set; }
    public string Ape1 { get; set; }
    public string Ape2 { get; set; }

    public string NoControl { get; set; }
    
    public string Carrera { get; set; }
    
    public string Correo { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    public string NombreCompleto
    {
        get => $"{Nombre} {Ape1} {Ape2}";
    }

    public EgresadoViewModel(int idUsuario, int idEgresado)
    {
        this.idEgresado = idEgresado;
        this.idUsuario = idUsuario;
    }
}