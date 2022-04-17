using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Galerium
    {
        public Galerium()
        {
            ImagenGaleria = new HashSet<ImagenGalerium>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime? Fecha { get; set; }

        public virtual ICollection<ImagenGalerium> ImagenGaleria { get; set; }
    }
}
