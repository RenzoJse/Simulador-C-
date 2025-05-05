using Microsoft.EntityFrameworkCore;
using ObjectSim.Domain;

namespace ObjectSim.DataAccess.Repositories;

public class ClassRepository(DbContext context) : Repository<Class>(context)
{
    private readonly DbSet<Class> _classes = context.Set<Class>();

    public override Class? Get(Func<Class, bool> filter)
    {
        return _classes
            .Include(c => c.Attributes)
            .Include(c => c.Methods)
            .Include(c => c.Parent)
            .AsEnumerable()
            .FirstOrDefault(filter);
    }
}
