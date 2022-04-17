using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class TagImagen
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? IdImagen { get; set; }

        public virtual ImagenGalerium? IdImagenNavigation { get; set; }
    }
}
