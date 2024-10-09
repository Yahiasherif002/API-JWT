using System.Linq.Expressions;

namespace API_1.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        Task <IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
