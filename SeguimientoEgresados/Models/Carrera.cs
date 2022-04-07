using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            Egresados = new HashSet<Egresado>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Egresado> Egresados { get; set; }
    }
}
