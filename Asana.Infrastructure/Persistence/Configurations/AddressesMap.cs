using Asana.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asana.Infrastructure.Persistence.Configurations
{
    public class AddressesMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasQueryFilter(a => !a.IsDelete);

            builder
                .HasOne(a => a.State)
                 .WithMany(s => s.Addresses)
                 .HasForeignKey(a => a.StateName)
                 .HasPrincipalKey(a => a.Name);

            builder
                .HasOne(a => a.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CityName)
                .HasPrincipalKey(c => c.Name);
        }
    }

    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasQueryFilter(c => !c.IsDelete);

            builder
                .HasOne(c => c.State)
                .WithMany(s => s.Cities)
                .HasForeignKey(c => c.StateName)
                .HasPrincipalKey(s => s.Name);

        }
    }

    public class StateMap : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasQueryFilter(s => !s.IsDelete);
        }
    }
}
