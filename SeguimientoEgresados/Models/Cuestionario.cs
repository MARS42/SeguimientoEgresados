using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Cuestionario
    {
        public int Id { get; set; }
        public DateTime? UltimaAplicacion { get; set; }
        public DateTime ProximaAplicacion { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
