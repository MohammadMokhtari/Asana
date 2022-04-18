using Asana.Domain.Entities.Addresses;
using Asana.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<UserMediaFile> UserMediaFiles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
