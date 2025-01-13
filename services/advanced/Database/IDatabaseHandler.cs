using System.Linq.Expressions;

namespace Database;

public interface IDatabaseHandler
{
    Task<T> AddAsync<T>(T entity) where T : class;
    Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class;
    Task<IEnumerable<T>> GetAsync<T>(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[] includes) where T : class;
    Task<List<T>> GetAllAsync<T>() where T : class;
    Task UpdateAsync<T>(T entity) where T : class;
    Task DeleteAsync<T>(T entity) where T : class;
}
