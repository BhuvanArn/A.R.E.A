namespace Database.Dao;

public class DaoFactory
{
    private readonly DatabaseContext _context;

    public DaoFactory(DatabaseContext context)
    {
        _context = context;
    }

    public IGenericDao<T> CreateDao<T>() where T : class
    {
        return new GenericDao<T>(_context);
    }
}