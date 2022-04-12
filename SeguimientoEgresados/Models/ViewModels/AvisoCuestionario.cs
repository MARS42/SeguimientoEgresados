namespace SeguimientoEgresados.Models.ViewModels
{
    public class AvisoCuestionario
    {
        public readonly string Titulo;
        public readonly string Cuerpo;
        public readonly bool forceShow; 

        public AvisoCuestionario(string titulo, string cuerpo, bool forceShow)
        {
            Titulo = titulo;
            Cuerpo = cuerpo;
            this.forceShow = forceShow;
        }
    }
}
