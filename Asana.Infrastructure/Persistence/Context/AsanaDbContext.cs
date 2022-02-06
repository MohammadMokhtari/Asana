using Asana.Domain.Entities.Addresses;
using Asana.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Asana.Infrastructure.Persistence.Context
{
    public class AsanaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AsanaDbContext(DbContextOptions<AsanaDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }
        
    }
}
