using Core.Domain.Common;
using Core.Domain.Entities;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    //Entities
    public DbSet<Beneficiario> Beneficiarios { get; set; }
    public DbSet<CuentaAhorro> CuentaAhorros { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<TarjetaCredito> TarjetaCreditos { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedBy = "DefaultAppUser";
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.Now;
                    entry.Entity.LastModifiedBy = "DefaultAppUser";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Tables

        modelBuilder.Entity<Beneficiario>().ToTable("Beneficiarios");
        modelBuilder.Entity<CuentaAhorro>().ToTable("CuentaAhorros");
        modelBuilder.Entity<Prestamo>().ToTable("Prestamos");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<TarjetaCredito>().ToTable("TarjetaCreditos");

        #endregion

        #region PrimaryKeys

        modelBuilder.Entity<Beneficiario>().HasKey(x => x.Id);
        modelBuilder.Entity<CuentaAhorro>().HasKey(x => x.Id);
        modelBuilder.Entity<Prestamo>().HasKey(x => x.Id);
        modelBuilder.Entity<Product>().HasKey(x => x.Id);
        modelBuilder.Entity<TarjetaCredito>().HasKey(x => x.Id);


        #endregion

        #region RelationShips

        modelBuilder.Entity<Product>()
            .HasOne(P => P.CuentaAhorros)
            .WithOne(CA => CA.Product)
            .HasForeignKey<CuentaAhorro>(CA => CA.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(P => P.TarjetaCreditos)
            .WithOne(TC => TC.Product)
            .HasForeignKey<TarjetaCredito>(TC => TC.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(P => P.Prestamos)
            .WithOne(PS => PS.Product)
            .HasForeignKey<Prestamo>(PS => PS.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<TarjetaCredito>()
           .HasOne(P => P.Product)
           .WithOne(TC => TC.TarjetaCreditos)
           .HasForeignKey<Product>(TC => TC.IdProductType)
           .OnDelete(DeleteBehavior.Cascade);

        //beneficiarios
        modelBuilder.Entity<CuentaAhorro>()
                .HasMany<Beneficiario>(CA => CA.Beneficiarios)
                .WithOne(B => B.CuentaAhorro)
                .HasForeignKey(B => B.IdCuentaAhorro)
                .OnDelete(DeleteBehavior.Cascade);
        #endregion

        #region PropertyConfigurations

        #region Beneficiario
        modelBuilder.Entity<Beneficiario>()
            .Property(x => x.NumeroCuenta)
            .IsRequired();

        modelBuilder.Entity<Beneficiario>()
            .Property(x => x.IdUser)
            .IsRequired();
        #endregion

        #region CuentaAhorro

        modelBuilder.Entity<CuentaAhorro>()
            .Property(x => x.Balance)
            .IsRequired();

        modelBuilder.Entity<CuentaAhorro>()
            .Property(x => x.Principal)
            .IsRequired();

        modelBuilder.Entity<CuentaAhorro>()
            .Property(x => x.NumeroCuenta)
            .IsRequired();

        modelBuilder.Entity<CuentaAhorro>()
                .Property(x => x.IdProduct)
                .IsRequired(false);

        #endregion

        #region Prestamo

        modelBuilder.Entity<Prestamo>()
            .Property(x => x.Debe)
            .IsRequired();

        modelBuilder.Entity<Prestamo>()
            .Property(x => x.Monto)
            .IsRequired();

        modelBuilder.Entity<Prestamo>()
            .Property(x => x.Pago)
            .IsRequired();

        modelBuilder.Entity<Prestamo>()
            .Property(x => x.NumeroPrestamo)
            .IsRequired();

        #endregion

        #region Product

        modelBuilder.Entity<Product>()
            .Property(x => x.Primary)
            .IsRequired();


        modelBuilder.Entity<Product>()
            .Property(x => x.IdUser)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(x => x.IdProductType)
            .IsRequired(false);

        #endregion

        #region TarjetaCredito

        modelBuilder.Entity<TarjetaCredito>()
            .Property(x => x.Debe)
            .IsRequired();

        modelBuilder.Entity<TarjetaCredito>()
            .Property(x => x.Limite)
            .IsRequired();

        modelBuilder.Entity<TarjetaCredito>()
            .Property(x => x.Pago)
            .IsRequired();

        modelBuilder.Entity<TarjetaCredito>()
            .Property(x => x.NumeroTarjeta)
            .IsRequired();

        #endregion

        #endregion
    }
}