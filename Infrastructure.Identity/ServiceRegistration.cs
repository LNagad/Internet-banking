using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Context;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            #region "InMemory Database"
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options
                .UseInMemoryDatabase("IdentityDB"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options => options
                .UseSqlServer(config.GetConnectionString("IdentityConnection"), m => m
                .MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }
            #endregion

            #region "Identity"
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccesDenied";
            });
           

            services.AddAuthentication();
            #endregion

            #region services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IDashboradService, DashboardService>();
            services.AddTransient<IManageUserService, ManageUserService>();
            #endregion
        }
    }
}
