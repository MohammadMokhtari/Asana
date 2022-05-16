using Asana.Domain.Entities.Addresses;
using Asana.Domain.Entities.Categories;
using Asana.Domain.Entities.Media;
using Asana.Domain.Entities.Products;
using Asana.Domain.Entities.Token;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<UserMediaFile> UserMediaFiles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<MediaFile> MediaFiles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Color> Colors { get; set; }
        
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductMediaFile> ProductMediaFiles { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<ProductReviewHelpfulness> ProductReviewHelpfulnesses { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<ProductWarranti> ProductWarrantis { get; set; }

        public DbSet<SpecializedProductReview> SpecializedProductReviews { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
