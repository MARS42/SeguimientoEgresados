using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            Operaciones = new HashSet<Operacione>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Operacione> Operaciones { get; set; }
    }
}
