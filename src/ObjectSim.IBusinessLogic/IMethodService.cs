namespace ObjectSim.IBusinessLogic;

public interface IMethodService<T>
{
    T Create(T Entity);
    List<T> GetAll();
    bool Delete(Guid id);
    T GetById(Guid id);
    T Update(Guid id, T entity);
}
