using Asana.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Asana.Infrastructure.Persistence.Configurations
{
    public class ProductsMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasQueryFilter(p => !p.IsDelete);

            builder.HasMany(p => p.ProductTags)
                .WithMany(t => t.Products)
                .UsingEntity<Dictionary<string, object>>(
                "ProductSelectedTag",
                c => c
                .HasOne<ProductTag>()
                .WithMany()
                .HasForeignKey("ProdcutTagId")
                .OnDelete(DeleteBehavior.Cascade),
                c => c
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j.ToTable("ProductSelectedTag", "Production")
                );

            builder
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            builder
                .HasOne(p => p.Warranti)
                .WithMany(w => w.Products)
                .HasForeignKey(p => p.WarrantiId)
                .IsRequired();

            builder
                .HasOne(p => p.Color)
                .WithOne()
                .HasForeignKey<Product>(p => p.ColorId);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,3)");

            builder.Property(p => p.SpecialPrice)
              .HasColumnType("decimal(18,3)");

            builder
                .Property(p => p.PersianName)
                .IsRequired()
                .HasMaxLength(300);

            builder
            .Property(p => p.EnglishName)
            .IsRequired()
            .HasMaxLength(300);

            builder.Property(p => p.ShortDescription)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }

    public class ProductMediaFileMap : IEntityTypeConfiguration<ProductMediaFile>
    {
        public void Configure(EntityTypeBuilder<ProductMediaFile> builder)
        {
            builder.HasQueryFilter(p => !p.IsDelete);

            builder
                .HasOne(p => p.Product)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();

        }
    }

    public class ProductReviewHelpfulnessMap : IEntityTypeConfiguration<ProductReviewHelpfulness>
    {
        public void Configure(EntityTypeBuilder<ProductReviewHelpfulness> builder)
        {
            builder
                .HasOne(h => h.Review)
                .WithMany()
                .HasForeignKey(h => h.ProductReviewId);
        }
    }


    public class ProductReviewMap : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {

            builder.HasQueryFilter(p => !p.IsDelete);


            builder
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .IsRequired()
                .HasForeignKey(r => r.ProductId);

            builder
                .Property(r => r.ReviewText)
                .IsRequired()
                .HasMaxLength(800);

            builder.Property(r => r.Title)
                .IsRequired().HasMaxLength(200);

            builder.Property(r => r.Rating)
                .IsRequired();
        }
    }


    public class ProductWarrantiMap : IEntityTypeConfiguration<ProductWarranti>
    {
        public void Configure(EntityTypeBuilder<ProductWarranti> builder)
        {
            builder.HasQueryFilter(w => !w.IsDelete);

            builder
                .HasMany(w => w.Products)
                .WithOne(p => p.Warranti)
                .HasForeignKey(p => p.WarrantiId);

            builder
                .Property(w => w.Description)
                .IsRequired();

            builder
                .Property(w => w.WarrantiName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }


    public class SpecializedProductReviewMap : IEntityTypeConfiguration<SpecializedProductReview>
    {
        public void Configure(EntityTypeBuilder<SpecializedProductReview> builder)
        {
            builder
                .HasOne(w => w.Product)
                .WithOne(p => p.SpecializedReview)
                .IsRequired();

            builder
                .Property(s => s.SpecializedReviewText)
                .IsRequired();
        }
    }

    public class ProductTagMap : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {

            builder
                .HasMany(w => w.Products)
                .WithMany(p => p.ProductTags);

            builder.Property(t => t.Name)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
