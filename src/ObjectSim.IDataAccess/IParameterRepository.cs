using System.Linq.Expressions;

namespace ObjectSim.IDataAccess;
public interface IParameterRepository<TEntity>
       where TEntity : class
{
    bool Exist(Expression<Func<TEntity, bool>> expression);

    TEntity Add(TEntity entity);
    IEnumerable<TEntity> GetAll();

    TEntity Get(Expression<Func<TEntity, bool>> predicate);

    void Remove(TEntity entity);

    void Update(TEntity entity);
    TEntity GetById(int id);
}
