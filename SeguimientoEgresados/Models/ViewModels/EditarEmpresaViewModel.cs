namespace SeguimientoEgresados.Models.ViewModels;

public class EditarEmpresaViewModel
{
    public Usuario usuario { get; set; }
    public Empresa empresa { get; set; }

    public EditarEmpresaViewModel(Usuario u, Empresa e)
    {
        usuario = u;
        empresa = e;
    }
}