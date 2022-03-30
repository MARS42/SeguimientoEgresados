using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SeguimientoEgresados.Models
{
    public partial class SeguimientoEgresadosContext : DbContext
    {
        public SeguimientoEgresadosContext()
        {
        }

        public SeguimientoEgresadosContext(DbContextOptions<SeguimientoEgresadosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Operacione> Operaciones { get; set; } = null!;
        public virtual DbSet<RolOperacion> RolOperacions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=SeguimientoEgresados;Trusted_Connection=False;User Id=SA;Password=3549355sql!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasIndex(e => e.Id, "Empresas_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Rfc, "Empresas_rfc_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Colonia)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("colonia");

                entity.Property(e => e.CorreoEmpresa)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("correo_empresa");

                entity.Property(e => e.Cp).HasColumnName("cp");

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("domicilio");

                entity.Property(e => e.Estado)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("municipio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Pais)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pais");

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("razon_social");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.Website)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("website");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Empresas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Empresas_Usuarios_id_fk");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasIndex(e => e.Id, "Modulos_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Operacione>(entity =>
            {
                entity.HasIndex(e => e.Id, "Operaciones_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdModulo).HasColumnName("idModulo");

                entity.Property(e => e.Nombe)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("nombe");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.Operaciones)
                    .HasForeignKey(d => d.IdModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Operaciones_Modulos_id_fk");
            });

            modelBuilder.Entity<RolOperacion>(entity =>
            {
                entity.ToTable("Rol_Operacion");

                entity.HasIndex(e => e.Id, "Rol_Operacion_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdOperacion).HasColumnName("idOperacion");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.HasOne(d => d.IdOperacionNavigation)
                    .WithMany(p => p.RolOperacions)
                    .HasForeignKey(d => d.IdOperacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Rol_Operacion_idOpe__fk");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolOperacions)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Rol_Operacion_idRol__fk");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Id, "Roles_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email, "Usuarios_email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "Usuarios_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Email)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Password)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Password512)
                    .HasMaxLength(64)
                    .HasColumnName("password512")
                    .IsFixedLength();

                entity.Property(e => e.Salt).HasColumnName("salt");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Usuarios_Roles_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
