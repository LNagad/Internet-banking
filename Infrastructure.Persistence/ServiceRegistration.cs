using Core.Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Contexts

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
        }


        #endregion

        #region Repositories

        //repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBeneficiarioRepository, BeneficiarioRepository>();
            services.AddTransient<ICuentaAhorroRepository, CuentaAhorroRepository>();
            services.AddTransient<IPrestamoRepository, PrestamoRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ITarjetaCreditoRepository, TarjetaCreditoRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        #endregion
    }
}