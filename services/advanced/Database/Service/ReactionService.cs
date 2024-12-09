using System.Linq.Expressions;
using Database.Dao;
using Database.Entities;

namespace Database.Service;

public class ReactionService
{
    private readonly IGenericDao<Reaction> _reactionDao;

    public ReactionService(DaoFactory daoFactory)
    {
        _reactionDao = daoFactory.CreateDao<Reaction>();
    }

    public async Task CreateReactionAsync(Reaction reaction)
    {
        if (reaction == null)
            throw new ArgumentNullException(nameof(reaction));

        await _reactionDao.AddAsync(reaction);
        await _reactionDao.SaveChangesAsync();
    }

    public async Task<Reaction> GetReactionByIdAsync(Guid id)
    {
        return await _reactionDao.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reaction>> GetAllReactionsAsync()
    {
        return await _reactionDao.GetAllAsync();
    }

    public async Task UpdateReactionAsync(Reaction reaction)
    {
        if (reaction == null)
            throw new ArgumentNullException(nameof(reaction));

        _reactionDao.Update(reaction);
        await _reactionDao.SaveChangesAsync();
    }

    public async Task DeleteReactionAsync(Guid id)
    {
        var reaction = await _reactionDao.GetByIdAsync(id);
        if (reaction == null)
            throw new KeyNotFoundException($"Reaction with ID {id} not found.");

        _reactionDao.Remove(reaction);
        await _reactionDao.SaveChangesAsync();
    }

    public async Task<IEnumerable<Reaction>> FindReactionsAsync(Expression<Func<Reaction, bool>> predicate)
    {
        return await _reactionDao.FindAsync(predicate);
    }
}