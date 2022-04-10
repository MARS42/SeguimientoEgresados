namespace SeguimientoEgresados.Models.ViewModels
{
    public class AvisoCuestionario
    {
        public readonly string Titulo;
        public readonly string Cuerpo;

        public AvisoCuestionario(string titulo, string cuerpo)
        {
            Titulo = titulo;
            Cuerpo = cuerpo;
        }
    }
}
