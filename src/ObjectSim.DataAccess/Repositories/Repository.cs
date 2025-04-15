using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Interface;

namespace ObjectSim.DataAccess.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    private readonly DbContext _context = context;

    public TEntity Add(TEntity element)
    {
        _entities.Add(element);

        _context.SaveChanges();

        return element;
    }
}
