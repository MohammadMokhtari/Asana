using Asana.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity , ISoftDeletable
    {
        IQueryable<TEntity> GetEntitiesQuery();

        Task<TEntity> GetEntityByIdAsync(long id);

        Task AddEntityAsync(TEntity entity);

        void UpdateEntity(TEntity entity);

        void UpdateRangeEntity(IEnumerable<TEntity> entities);

        void RemoveEntity(TEntity entity);

        Task RemoveEntityAsync(long id);

        Task SaveChangeAsync();

        void DeleteEntity(TEntity entity);  
    }
}
