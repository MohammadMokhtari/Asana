using Asana.Application.Common.Interfaces;
using Asana.Infrastructure.Identity;
using Asana.Infrastructure.Persistence.Context;
using Asana.Infrastructure.Persistence.Options;
using Asana.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asana.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext

            services.AddDbContext<AsanaDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("AsanaConnectionString"));
            });

            #endregion

            #region Identity 

            services.AddDefaultIdentity<ApplicationUser>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredUniqueChars = 0;

                config.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AsanaDbContext>()
                .AddDefaultTokenProviders();

            #endregion


           
            return services;
        }
    }
}
