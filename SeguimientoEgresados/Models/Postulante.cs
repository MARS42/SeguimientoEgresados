using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Postulante
    {
        public int Id { get; set; }
        public int IdEgresado { get; set; }
        public int IdVacante { get; set; }
        public string CvUrl { get; set; } = null!;
        public DateTime Fecha { get; set; }

        public virtual Egresado IdEgresadoNavigation { get; set; } = null!;
        public virtual Vacante IdVacanteNavigation { get; set; } = null!;
    }
}
