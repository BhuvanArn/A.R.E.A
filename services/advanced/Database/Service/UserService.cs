using System.Linq.Expressions;
using Database.Dao;
using Database.Entities;

namespace Database.Service;

public class UserService
{
    private readonly IGenericDao<User> _userDao;

    public UserService(DaoFactory daoFactory)
    {
        _userDao = daoFactory.CreateDao<User>();
    }
    
    public async Task CreateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        await _userDao.AddAsync(user);
        await _userDao.SaveChangesAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userDao.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userDao.GetAllAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _userDao.Update(user);
        await _userDao.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userDao.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        _userDao.Remove(user);
        await _userDao.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> FindUsersAsync(Expression<Func<User, bool>> predicate)
    {
        return await _userDao.FindAsync(predicate);
    }
}