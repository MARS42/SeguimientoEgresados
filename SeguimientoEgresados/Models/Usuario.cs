using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int IdRol { get; set; }

        public virtual Role IdRolNavigation { get; set; } = null!;
    }
}
