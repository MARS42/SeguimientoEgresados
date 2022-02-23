using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class RolOperacion
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdOperacion { get; set; }

        public virtual Operacione IdOperacionNavigation { get; set; } = null!;
        public virtual Role IdRolNavigation { get; set; } = null!;
    }
}
