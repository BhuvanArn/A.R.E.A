using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database.Dao;

public interface IGenericDao<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChangesAsync();
}
