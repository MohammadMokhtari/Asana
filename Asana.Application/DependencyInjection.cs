using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Asana.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            services.AddScoped<ILocationService, LocationService>();

            return services;
        }
    }
}
