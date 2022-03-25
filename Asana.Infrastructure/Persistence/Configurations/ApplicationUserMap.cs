using Asana.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asana.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasMany(a => a.Addresses)
                .WithOne()
                .HasForeignKey(a => a.UserId)
                .IsRequired();
        }
    }
}
