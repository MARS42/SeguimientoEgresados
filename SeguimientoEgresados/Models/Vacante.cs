using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Vacante
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Funciones { get; set; } = null!;
        public string Requisitos { get; set; } = null!;
        public string TipoContrato { get; set; } = null!;
        public string Modalidad { get; set; } = null!;
        public string Horario { get; set; } = null!;
        public string Ofertas { get; set; } = null!;
        public int IdEmpresa { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
    }
}
