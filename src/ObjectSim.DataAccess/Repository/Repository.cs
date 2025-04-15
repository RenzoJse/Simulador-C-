using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Interface;

namespace ObjectSim.DataAccess.Repository;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

}
