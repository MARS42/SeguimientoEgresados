using SeguimientoEgresados.Models;

namespace SeguimientoEgresados.Utils;

public static class CheckUserVerification
{
    public static bool EstaVerificado(this Usuario usuario)
    {
        return usuario.Verificado != null && usuario.Verificado.Equals("true");
    }
}