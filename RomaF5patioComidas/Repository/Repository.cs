using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;

namespace RomaF5patioComidas.Repository
{
    public  class Repository<T> : IRepository<T> where T : class
    {

        protected readonly RomaF5BdContext _dbContext;

        public Repository(RomaF5BdContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync() ?? throw new Exception();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id) ?? throw new NullReferenceException() ;
        }

        public void Update(T entity)
        {
             _dbContext.Set<T>().Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
