namespace EcomonedasUTN.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ecoMonedaModel : DbContext
    {
        public ecoMonedaModel()
            : base("name=ecoMonedaModel1")
        {
        }

        public virtual DbSet<BilleteraVirtual> BilleteraVirtual { get; set; }
        public virtual DbSet<Centro> Centro { get; set; }
        public virtual DbSet<Cupon> Cupon { get; set; }
        public virtual DbSet<CuponesDisponibles> CuponesDisponibles { get; set; }
        public virtual DbSet<DetalleCambio> DetalleCambio { get; set; }
        public virtual DbSet<EncCambio> EncCambio { get; set; }
        public virtual DbSet<historial> historial { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BilleteraVirtual>()
                .Property(e => e.idUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Centro>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Centro>()
                .Property(e => e.provincia)
                .IsUnicode(false);

            modelBuilder.Entity<Centro>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Centro>()
                .Property(e => e.idUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Centro>()
                .HasMany(e => e.EncCambio)
                .WithRequired(e => e.Centro)
                .HasForeignKey(e => e.idCentro)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cupon>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Cupon>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Cupon>()
                .HasMany(e => e.CuponesDisponibles)
                .WithRequired(e => e.Cupon)
                .HasForeignKey(e => e.idCupon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cupon>()
                .HasMany(e => e.historial)
                .WithRequired(e => e.Cupon)
                .HasForeignKey(e => e.idCupon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CuponesDisponibles>()
                .Property(e => e.idUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<EncCambio>()
                .Property(e => e.idUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<EncCambio>()
                .HasMany(e => e.DetalleCambio)
                .WithRequired(e => e.EncCambio)
                .HasForeignKey(e => e.idEncCambio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<historial>()
                .Property(e => e.idUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.color)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.DetalleCambio)
                .WithRequired(e => e.Material)
                .HasForeignKey(e => e.idMaterial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.BilleteraVirtual)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.idUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Centro)
                .WithOptional(e => e.Usuario)
                .HasForeignKey(e => e.idUsuario);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.CuponesDisponibles)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.idUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.EncCambio)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.idUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.historial)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.idUsuario)
                .WillCascadeOnDelete(false);
        }
    }
}
