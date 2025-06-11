using Microsoft.EntityFrameworkCore;
using ObjectSim.Domain;

namespace ObjectSim.DataAccess.Repositories;

public class MethodRepository(DbContext context) : Repository<Method>(context)
{
    private readonly DbSet<Method> _methods = context.Set<Method>();

    public override Method? Get(Func<Method, bool> filter)
    {
        return _methods
            .Include(m => m.Parameters)
            .Include(m => m.MethodsInvoke)
            .AsEnumerable()
            .FirstOrDefault(filter);
    }
}
