using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class IntervalosCuestionario
    {
        public string Nombre { get; set; } = null!;
        public int Meses { get; set; }
        public int? IdRol { get; set; }
        public int Id { get; set; }

        public virtual Role? IdRolNavigation { get; set; }
    }
}
