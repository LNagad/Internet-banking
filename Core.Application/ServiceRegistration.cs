using Core.Application.Interfaces.Services;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICuentaAhorroService, CuantaAhorroService>();
            services.AddTransient<ITarjetaCreditoService, TarjetaCreditoService>();
            services.AddTransient<IPrestamoService, PrestamoService>();
            services.AddTransient<IBeneficiarioService, BeneficiarioService>();
            services.AddTransient<IPagosService, PagosService>();
            services.AddTransient<ITransactionService, TransactionService>();
            #endregion
        }
    }
}
