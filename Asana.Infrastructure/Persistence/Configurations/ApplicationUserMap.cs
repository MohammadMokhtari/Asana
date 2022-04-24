using Asana.Domain.Entities.Media;
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
            builder
                .HasOne(a => a.MediaFile)
                .WithOne()
                .HasForeignKey<UserMediaFile>(m => m.UserId);

            builder
                .HasMany(a => a.RefreshToken)
                .WithOne()
                .HasForeignKey(r => r.UserId);

            builder
                .HasMany(a => a.Reviews)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .IsRequired();
        }
    }
}
