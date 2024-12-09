using System.Linq.Expressions;
using Database.Dao;

namespace Database.Service;

public class ServiceService
{
    private readonly IGenericDao<Entities.Service> _serviceDao;

    public ServiceService(DaoFactory daoFactory)
    {
        _serviceDao = daoFactory.CreateDao<Entities.Service>();
    }

    public async Task CreateServiceAsync(Entities.Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        await _serviceDao.AddAsync(service);
        await _serviceDao.SaveChangesAsync();
    }

    public async Task<Entities.Service> GetServiceByIdAsync(Guid id)
    {
        return await _serviceDao.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Entities.Service>> GetAllServicesAsync()
    {
        return await _serviceDao.GetAllAsync();
    }

    public async Task UpdateServiceAsync(Entities.Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        _serviceDao.Update(service);
        await _serviceDao.SaveChangesAsync();
    }

    public async Task DeleteServiceAsync(Guid id)
    {
        var service = await _serviceDao.GetByIdAsync(id);
        if (service == null)
            throw new KeyNotFoundException($"Service with ID {id} not found.");

        _serviceDao.Remove(service);
        await _serviceDao.SaveChangesAsync();
    }

    public async Task<IEnumerable<Entities.Service>> FindServicesAsync(Expression<Func<Entities.Service, bool>> predicate)
    {
        return await _serviceDao.FindAsync(predicate);
    }
}