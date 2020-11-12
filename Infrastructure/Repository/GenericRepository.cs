using FinanceManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected FinanceManagerContext _context;
        private DbSet<T> _entities;

        public GenericRepository(FinanceManagerContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<bool> Delete(T value)
        {
            bool isDeleted = false;

            if (_entities.Contains(value))
            {
                _entities.Remove(value);
                await _context.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<List<T>> GetAll()
        {
            return await _entities
                            .Select(x => x)
                            .ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await _entities
                            .FindAsync(id);
        }

        public async Task<T> Insert(T value)
        {
            _entities.Add(value);
            await _context.SaveChangesAsync();
            return value;
        }

        public async Task<bool> Update(T value)
        {
            bool isUpdated = false;

            var dbEntity = await _entities
                            .FirstOrDefaultAsync(entity =>
                                (long)entity
                                    .GetType()
                                    .GetProperty($"{typeof(T).Name}Id")
                                    .GetValue(entity, null)
                                    ==
                                (long)value.GetType()
                                    .GetProperty($"{typeof(T).Name}Id")
                                    .GetValue(entity, null));

            if (!(dbEntity is null))
            {
                _context.Entry(dbEntity).CurrentValues.SetValues(value);
                await _context.SaveChangesAsync();
                isUpdated = true;
            }

            return isUpdated;
        }
    }
}
