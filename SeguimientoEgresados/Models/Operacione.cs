using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Operacione
    {
        public Operacione()
        {
            RolOperacions = new HashSet<RolOperacion>();
        }

        public int Id { get; set; }
        public string Nombe { get; set; } = null!;
        public int IdModulo { get; set; }

        public virtual Modulo IdModuloNavigation { get; set; } = null!;
        public virtual ICollection<RolOperacion> RolOperacions { get; set; }
    }
}
