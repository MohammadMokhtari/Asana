using Asana.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asana.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<AsanaDbContext>(option =>
            {

                option.UseSqlServer(configuration.GetConnectionString("AsanaConnectionString"));

            });

            return services;
        }
    }
}
