using Asana.Application.Common.Interfaces;
using Asana.Domain.Interfaces;
using Asana.Infrastructure.Identity;
using Asana.Infrastructure.Persistence.Context;
using Asana.Infrastructure.Persistence.Options;
using Asana.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        LifetimeValidator = TokenLifetimeValidator.Validate,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidIssuer = configuration["Jwt:Issuer"],
                        IssuerSigningKey = 
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    };
                });

            #endregion

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IIdentityService, UserService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Email));

            return services;
        }
    }
}
