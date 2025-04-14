using System.Linq.Expressions;

namespace ObjectSim.IDataAccess;

public interface IMethodRepository<TEntity>
       where TEntity : class
{
    TEntity Add(TEntity entity);
    IEnumerable<TEntity> GetAll();

    TEntity Get(Expression<Func<TEntity, bool>> predicate);

    void Remove(TEntity entity);

    void Update(TEntity entity);
    TEntity GetById(int id);
    bool Exist(Expression<Func<TEntity, bool>> expression);
}
