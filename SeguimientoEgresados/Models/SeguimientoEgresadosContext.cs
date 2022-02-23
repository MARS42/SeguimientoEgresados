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
            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasIndex(e => e.Id, "Modulos_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(1)
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
                    .HasMaxLength(1)
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
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Id, "Usuarios_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Password)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("password");

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
