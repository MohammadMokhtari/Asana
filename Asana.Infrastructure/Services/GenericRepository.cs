using Asana.Domain.Entities.Common;
using Asana.Domain.Interfaces;
using Asana.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Infrastructure.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, ISoftDeletable
    {
        private readonly AsanaDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(AsanaDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetEntitiesQuery()
        {
            return _dbset.AsQueryable();
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = entity.CreatedOn;
            await _dbset.AddAsync(entity);
        }

        public async Task<TEntity> GetEntityByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public void UpdateEntity(TEntity entity)
        {
            _dbset.Update(entity);
        }


        public void RemoveEntity(TEntity entity)
        {
            entity.IsDelete = true;
            UpdateEntity(entity);
        }

        public async Task RemoveEntity(int id)
        {
            var entity = await GetEntityByIdAsync(id);
            RemoveEntity(entity);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
