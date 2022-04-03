using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Cuestionario
    {
        public Cuestionario()
        {
            Empresas = new HashSet<Empresa>();
        }

        public int Id { get; set; }
        public DateTime? UltimaAplicacion { get; set; }
        public DateTime ProximaAplicacion { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
