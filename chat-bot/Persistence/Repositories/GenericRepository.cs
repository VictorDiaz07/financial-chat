using Chat.Persistence.Contexts;
using ChatBot.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Persistence.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DefaultContext _context;
        private DbSet<T> _entities;

        public GenericRepository(DefaultContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        protected virtual DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public IQueryable<T> Table => Entities;

        public async Task DeleteAsync(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task InsertAsync(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            await _context.SaveChangesAsync();
        }
    }
}
