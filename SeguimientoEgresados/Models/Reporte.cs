using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Reporte
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Area { get; set; } = null!;
        public string? Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Revisado { get; set; } = null!;
    }
}
