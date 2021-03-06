using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Role
    {
        public Role()
        {
            IntervalosCuestionarios = new HashSet<IntervalosCuestionario>();
            RolOperacions = new HashSet<RolOperacion>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<IntervalosCuestionario> IntervalosCuestionarios { get; set; }
        public virtual ICollection<RolOperacion> RolOperacions { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
