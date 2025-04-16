using System.Linq.Expressions;

namespace ObjectSim.IDataAccess;
public interface IParameterRepository<TEntity>
       where TEntity : class
{
    TEntity Add(TEntity entity);
    bool Exist(Expression<Func<TEntity, bool>> expression);
}
