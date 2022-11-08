using Core.Domain.Entities;
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
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Tables

            modelBuilder.Entity<Beneficiario>().ToTable("Beneficiarios");
            modelBuilder.Entity<CuentaAhorro>().ToTable("CuentaAhorros");
            modelBuilder.Entity<Prestamo>().ToTable("Prestamos");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<TarjetaCredito>().ToTable("TarjetaCreditos");
            modelBuilder.Entity<User>().ToTable("Users");

        #endregion
        
        #region PrimaryKeys
        
            modelBuilder.Entity<Beneficiario>().HasKey(x => x.Id);
            modelBuilder.Entity<CuentaAhorro>().HasKey(x => x.Id);
            modelBuilder.Entity<Prestamo>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<TarjetaCredito>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            
        #endregion
        
        #region RelationShips
        
            modelBuilder.Entity<User>()
                .HasMany<Product>(user => user.Products)
                .WithOne(P => P.User)
                .HasForeignKey(product => product.IdUser)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Product>()
                .HasMany<CuentaAhorro>( P => P.CuentaAhorros)
                .WithOne( CA => CA.Product)
                .HasForeignKey( CA => CA.IdProduct)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Product>()
                .HasMany<TarjetaCredito>( P => P.TarjetaCreditos)
                .WithOne( TC => TC.Product)
                .HasForeignKey( TC => TC.Idproduct)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Product>()
                .HasMany<Prestamo>( P => P.Prestamos)
                .WithOne( PS => PS.Product)
                .HasForeignKey( PS => PS.IdProduct) 
                .OnDelete(DeleteBehavior.NoAction);
        
        #endregion
        
        #region PropertyConfigurations

            #region Beneficiario
            modelBuilder.Entity<Beneficiario>()
                .Property(x => x.IdAccount)
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

            #endregion
            
            #region Prestamo

            modelBuilder.Entity<Prestamo>()
                .Property(x => x.Balance)
                .IsRequired();
            
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
                .Property(x => x.IdProduct)
                .IsRequired();
            
            modelBuilder.Entity<Product>()
                .Property(x => x.IdUser)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(x => x.IdProductType)
                .IsRequired();

            #endregion
            
            #region TarjetaCredito
            
            modelBuilder.Entity<TarjetaCredito>()
                .Property(x => x.Balance)
                .IsRequired();
            
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
            
            #region User
            
            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(x => x.Phone)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired();
            
            
            modelBuilder.Entity<User>()
                .Property(x => x.Status)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(x => x.UserType)
                .IsRequired();
            
            #endregion

            #endregion
    }
}