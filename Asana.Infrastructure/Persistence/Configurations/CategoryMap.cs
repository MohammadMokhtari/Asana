using Asana.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asana.Infrastructure.Persistence.Configurations
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasQueryFilter(c => !c.IsDelete);

            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.Categories)
                .HasForeignKey(c => c.ParentId);

            builder
                .Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
             .Property(c => c.Description)
             .HasMaxLength(800)
             .IsRequired();

            builder
             .HasOne(c => c.MediaFile)
             .WithOne()
             .HasForeignKey<Category>(c => c.MediaFileId);
        }
    }
}
