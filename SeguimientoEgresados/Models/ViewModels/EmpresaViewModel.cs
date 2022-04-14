namespace SeguimientoEgresados.Models.ViewModels;

public class EmpresaViewModel
{
    public readonly int idUsuario, idEmpresa;
    
    public string Nombre { get; set; }
    public string Rfc { get; set; }
    public string Correo { get; set; }
    
    public string Telefono { get; set; }
    
    public string NombreRep { get; set; }
    public string Ape1Rep { get; set; }
    public string Ape2Rep { get; set; }
    
    public string Representante
    {
        get => $"{NombreRep} {Ape1Rep} {Ape2Rep}";
    }
    
    public DateTime FechaRegistro { get; set; }

    public EmpresaViewModel(int idUsuario, int idEmpresa)
    {
        this.idEmpresa = idEmpresa;
        this.idUsuario = idUsuario;
    }
}