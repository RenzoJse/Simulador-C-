namespace ObjectSim.IDataAccess;
public interface IParameterRepository<TEntity>
       where TEntity : class
{
    TEntity Add(TEntity entity);
}
