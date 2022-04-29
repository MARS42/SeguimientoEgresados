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

        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<Convenio> Convenios { get; set; } = null!;
        public virtual DbSet<Cuestionario> Cuestionarios { get; set; } = null!;
        public virtual DbSet<Egresado> Egresados { get; set; } = null!;
        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<EstadosCivile> EstadosCiviles { get; set; } = null!;
        public virtual DbSet<Galerium> Galeria { get; set; } = null!;
        public virtual DbSet<Genero> Generos { get; set; } = null!;
        public virtual DbSet<ImagenGalerium> ImagenGaleria { get; set; } = null!;
        public virtual DbSet<IntervalosCuestionario> IntervalosCuestionarios { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<Operacione> Operaciones { get; set; } = null!;
        public virtual DbSet<Reporte> Reportes { get; set; } = null!;
        public virtual DbSet<RolOperacion> RolOperacions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TagImagen> TagImagens { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Vacante> Vacantes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=seguimiento-egresados-itc.database.windows.net;Database=SeguimientoEgresados;User=SE;Password=3549355sql!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.HasIndex(e => e.Id, "Carreras_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Nombre, "Carreras_nombre_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Convenio>(entity =>
            {
                entity.HasIndex(e => e.Id, "Convenios_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .HasColumnName("password")
                    .IsFixedLength();

                entity.Property(e => e.Salt).HasColumnName("salt");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Convenios)
                    .HasForeignKey(d => d.IdEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Convenios_Empresas_id_fk");
            });

            modelBuilder.Entity<Cuestionario>(entity =>
            {
                entity.HasIndex(e => e.Id, "Cuestionarios_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.ProximaAplicacion).HasColumnName("proxima_aplicacion");

                entity.Property(e => e.UltimaAplicacion).HasColumnName("ultima_aplicacion");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Cuestionarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Cuestionarios_Usuarios_id_fk");
            });

            modelBuilder.Entity<Egresado>(entity =>
            {
                entity.HasIndex(e => e.Id, "Egresados_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Colonia)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("colonia");

                entity.Property(e => e.Cp).HasColumnName("cp");

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("domicilio");

                entity.Property(e => e.Estado)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.EstadoNacimiento)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("estadoNacimiento");

                entity.Property(e => e.FechaEgreso).HasColumnName("fechaEgreso");

                entity.Property(e => e.FechaInicio).HasColumnName("fechaInicio");

                entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento");

                entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");

                entity.Property(e => e.IdEstadoCivil).HasColumnName("id_estadoCivil");

                entity.Property(e => e.IdGenero).HasColumnName("id_genero");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("municipio");

                entity.Property(e => e.MunicipioNacimiento)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("municipioNacimiento");

                entity.Property(e => e.NControl)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nControl");

                entity.Property(e => e.Pais)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pais");

                entity.Property(e => e.PaisNacimiento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("paisNacimiento");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.HasOne(d => d.IdCarreraNavigation)
                    .WithMany(p => p.Egresados)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Egresados_Carreras_id_fk");

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Egresados)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Egresados_EstadosCiviles_id_fk");

                entity.HasOne(d => d.IdGeneroNavigation)
                    .WithMany(p => p.Egresados)
                    .HasForeignKey(d => d.IdGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Egresados_Generos_id_fk");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Egresados)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Egresados_Usuarios_id_fk");
            });

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

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("descripcion")
                    .HasDefaultValueSql("('Sin descripción')");

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("domicilio");

                entity.Property(e => e.Estado)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");

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

                entity.HasOne(d => d.IdConvenioNavigation)
                    .WithMany(p => p.Empresas)
                    .HasForeignKey(d => d.IdConvenio)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Empresas_Convenios_id_fk");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Empresas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Empresas_Usuarios_id_fk");
            });

            modelBuilder.Entity<EstadosCivile>(entity =>
            {
                entity.HasIndex(e => e.Nombre, "EstadosCiviles_estadoCivil_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "EstadosCiviles_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Galerium>(entity =>
            {
                entity.HasIndex(e => e.Id, "Galeria_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasIndex(e => e.Id, "Generos_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<ImagenGalerium>(entity =>
            {
                entity.HasIndex(e => e.Id, "ImagenGaleria_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.IdAlbum).HasColumnName("id_album");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Url)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.Property(e => e.UrlThumb)
                    .IsUnicode(false)
                    .HasColumnName("url_thumb");

                entity.HasOne(d => d.IdAlbumNavigation)
                    .WithMany(p => p.ImagenGaleria)
                    .HasForeignKey(d => d.IdAlbum)
                    .HasConstraintName("ImagenGaleria_Galeria_id_fk");
            });

            modelBuilder.Entity<IntervalosCuestionario>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("IntervalosCuestionario");

                entity.HasIndex(e => e.Nombre, "IntervalosCuestionario_nombre_uindex")
                    .IsUnique();

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Meses).HasColumnName("meses");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("IntervalosCuestionario_Roles_id_fk");
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

            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Area)
                    .HasMaxLength(1024)
                    .HasColumnName("area");

                entity.Property(e => e.Descripcion).HasColumnName("descripcion");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Revisado)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("revisado");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .HasColumnName("titulo");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .HasColumnName("usuario");
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

            modelBuilder.Entity<TagImagen>(entity =>
            {
                entity.ToTable("TagImagen");

                entity.HasIndex(e => e.Id, "TagImagen_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdImagen).HasColumnName("id_imagen");

                entity.HasOne(d => d.IdImagenNavigation)
                    .WithMany(p => p.TagImagens)
                    .HasForeignKey(d => d.IdImagen)
                    .HasConstraintName("TagImagen_ImagenGaleria_id_fk");
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

                entity.Property(e => e.UrlImg)
                    .IsUnicode(false)
                    .HasColumnName("url_img");

                entity.Property(e => e.Verificado)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("verificado")
                    .HasDefaultValueSql("('false')");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Usuarios_Roles_id_fk");
            });

            modelBuilder.Entity<Vacante>(entity =>
            {
                entity.HasIndex(e => e.Id, "Vacantes_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.Funciones)
                    .IsUnicode(false)
                    .HasColumnName("funciones");

                entity.Property(e => e.Horario)
                    .IsUnicode(false)
                    .HasColumnName("horario");

                entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");

                entity.Property(e => e.Modalidad)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("modalidad");

                entity.Property(e => e.Ofertas)
                    .IsUnicode(false)
                    .HasColumnName("ofertas");

                entity.Property(e => e.Requisitos)
                    .IsUnicode(false)
                    .HasColumnName("requisitos");

                entity.Property(e => e.TipoContrato)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("tipo_contrato");

                entity.Property(e => e.Titulo)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Vacantes)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("Vacantes_Empresas_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
