using System.Linq.Expressions;
using Database.Dao;
using Action = Database.Entities.Action;

namespace Database.Service;

public class ActionService
{
    private readonly IGenericDao<Action> _actionDao;

    public ActionService(DaoFactory daoFactory)
    {
        _actionDao = daoFactory.CreateDao<Action>();
    }

    public async Task CreateActionAsync(Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        await _actionDao.AddAsync(action);
        await _actionDao.SaveChangesAsync();
    }

    public async Task<Action> GetActionByIdAsync(Guid id)
    {
        return await _actionDao.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Action>> GetAllActionsAsync()
    {
        return await _actionDao.GetAllAsync();
    }

    public async Task UpdateActionAsync(Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        _actionDao.Update(action);
        await _actionDao.SaveChangesAsync();
    }

    public async Task DeleteActionAsync(Guid id)
    {
        var action = await _actionDao.GetByIdAsync(id);
        if (action == null)
            throw new KeyNotFoundException($"Action with ID {id} not found.");

        _actionDao.Remove(action);
        await _actionDao.SaveChangesAsync();
    }

    public async Task<IEnumerable<Action>> FindActionsAsync(Expression<Func<Action, bool>> predicate)
    {
        return await _actionDao.FindAsync(predicate);
    }
}