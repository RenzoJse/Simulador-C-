using System.Linq.Expressions;

namespace ObjectSim.IDataAccess;
public interface ILocalVariableRepository<TEntity>
       where TEntity : class
{
    bool Exist(Expression<Func<TEntity, bool>> expression);

    TEntity Add(TEntity entity);
}
