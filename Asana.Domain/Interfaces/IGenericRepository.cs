using Asana.Domain.Entities.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity , ISoftDeletable
    {
        IQueryable<TEntity> GetEntitiesQuery();

        Task<TEntity> GetEntityByIdAsync(int id);

        Task AddEntityAsync(TEntity entity);

        void UpdateEntity(TEntity entity);

        void RemoveEntity(TEntity entity);

        Task RemoveEntity(int id);

        Task SaveChangeAsync();
    }
}
