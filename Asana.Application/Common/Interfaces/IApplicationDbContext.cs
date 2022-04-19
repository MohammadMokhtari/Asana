using Asana.Domain.Entities.Addresses;
using Asana.Domain.Entities.Media;
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
