using System.Linq.Expressions;
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

    public TEntity Update(TEntity element)
    {
        _entities.Update(element);

        _context.SaveChanges();

        return element;
    }

    public virtual TEntity? Get(Func<TEntity, bool> filter)
    {
        return _entities.FirstOrDefault(filter);
    }

    public virtual List<TEntity> GetAll(Func<TEntity, bool> filter)
    {
        return _entities.Where(filter).ToList();
    }

    public bool Exists(Expression<Func<TEntity, bool>> filter)
    {
        return _entities.Any(filter);
    }

    public void Delete(TEntity element)
    {
        _entities.Remove(element);

        _context.SaveChanges();
    }
}
