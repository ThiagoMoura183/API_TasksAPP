using Infra.Persistency;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infra.Repository.UnitOfWork
{
    public class BaseRepository<T>(TasksDbContext context) : IBaseRepository<T> where T : class {

        private readonly TasksDbContext _context = context;

        public async Task<T> Create(T command) {
            await _context.Set<T>().AddAsync(command);
            return command;
        }

        public Task Delete(Guid id) {
            _context.Remove(id);
            return Task.CompletedTask;
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression) {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public IEnumerable<T> GetAll() {
            // Utilizando spread (..) para explodir o resultado em lista individual
            return [.. _context.Set<T>().ToList()];
        }

        public async Task<T> Update(T commandUpdate) {
            _context.Set<T>().Update(commandUpdate);
            return commandUpdate;
        }
    }
}
