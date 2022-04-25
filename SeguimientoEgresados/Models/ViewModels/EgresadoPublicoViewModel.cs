using SeguimientoEgresados.Utils;

namespace SeguimientoEgresados.Models.ViewModels;

public class EgresadoPublicoViewModel
{
    public string Nombres { get; set; }
    public string ApeP { get; set; }
    public string ApeM { get; set; }

    public string NombreCompleto => $"{ApeP} {ApeM} {Nombres}";

    public string NoControl { get; set; }

    public string Carrera { get; set; }

    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public string Generación()
    {
        if (FechaInicio == null || FechaFin == null)
            return "No especificada";

        return $"{FechaInicio?.Month.ToMonth()} {FechaInicio?.Year} - {FechaFin?.Month.ToMonth()} {FechaFin?.Year}";
    }
    
}