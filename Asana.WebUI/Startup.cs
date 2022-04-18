using Asana.Application;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Mappings;
using Asana.Infrastructure;
using Asana.WebUI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Asana.WebUI
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);

            services.AddApplication();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IHostService, HostService>();

            services.AddControllers().AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.AddCors(option =>
            {
                option.AddPolicy("asanaClientApp", builder => {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    ;
                    
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("asanaClientApp");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
