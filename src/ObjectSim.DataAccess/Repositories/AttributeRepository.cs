using Microsoft.EntityFrameworkCore;

namespace ObjectSim.DataAccess.Repositories;
public class AttributeRepository(DbContext context) : Repository<Domain.Attribute>(context)
{
    private readonly DbSet<Domain.Attribute> _attributes = context.Set<Domain.Attribute>();
    public override Domain.Attribute? Get(Func<Domain.Attribute, bool> filter)
    {
        return _attributes
            .Include(a => a.DataType)
            .AsEnumerable()
            .FirstOrDefault(filter);
    }
}
