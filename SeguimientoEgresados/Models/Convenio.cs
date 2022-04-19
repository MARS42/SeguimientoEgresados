using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Convenio
    {
        public Convenio()
        {
            Empresas = new HashSet<Empresa>();
        }

        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public byte[]? Password { get; set; }
        public Guid? Salt { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
