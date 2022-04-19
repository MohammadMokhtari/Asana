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
using System.Reflection;
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


            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AsanaDbContext>());

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

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                //LifetimeValidator = TokenLifetimeValidator.Validate,
                ValidAudience = configuration["JWtBearer:Audience"],
                ValidIssuer = configuration["JWtBearer:Issuer"],
                IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWtBearer:Key"])),
            };

            services.AddSingleton(tokenValidationParameters);

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
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            #endregion

            #region Services

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<ISecurityService, SecurityService>();

            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Email));
            services.Configure<BearerTokensOptions>(configuration.GetSection(BearerTokensOptions.JWtBearer));

            #endregion

            return services;
        }
    }
}
