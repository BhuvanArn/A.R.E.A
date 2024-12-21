using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseHandler : IDatabaseHandler
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public DatabaseHandler(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<List<T>> GetAllAsync<T>() where T : class
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().ToListAsync();
    }

    public async Task UpdateAsync<T>(T entity) where T : class
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}
