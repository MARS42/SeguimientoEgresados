﻿using System;
using System.Collections.Generic;

namespace SeguimientoEgresados.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Empresas = new HashSet<Empresa>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int IdRol { get; set; }
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public byte[]? Password512 { get; set; }
        public Guid? Salt { get; set; }

        public virtual Role IdRolNavigation { get; set; } = null!;
        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
