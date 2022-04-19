using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Convenios = new HashSet<Convenio>();
            Vacantes = new HashSet<Vacante>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Rfc { get; set; } = null!;
        public string RazonSocial { get; set; } = null!;
        public string Domicilio { get; set; } = null!;
        public string Colonia { get; set; } = null!;
        public int Cp { get; set; }
        public string Pais { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string? Municipio { get; set; }
        public string CorreoEmpresa { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Website { get; set; }
        public int IdUsuario { get; set; }
        public int? IdConvenio { get; set; }
        public string? Descripcion { get; set; }

        public virtual Convenio? IdConvenioNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<Convenio> Convenios { get; set; }
        public virtual ICollection<Vacante> Vacantes { get; set; }
    }
}
