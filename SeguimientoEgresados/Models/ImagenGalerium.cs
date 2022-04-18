using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class ImagenGalerium
    {
        public ImagenGalerium()
        {
            TagImagens = new HashSet<TagImagen>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Url { get; set; } = null!;
        public DateTime? Fecha { get; set; }
        public int? IdAlbum { get; set; }
        public string? UrlThumb { get; set; }

        public virtual Galerium? IdAlbumNavigation { get; set; }
        public virtual ICollection<TagImagen> TagImagens { get; set; }
    }
}
