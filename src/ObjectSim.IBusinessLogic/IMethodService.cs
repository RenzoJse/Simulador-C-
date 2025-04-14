namespace ObjectSim.IBusinessLogic;

public interface IMethodService<T>
{
    T Create(T Entity);
    List<T> GetAll();
    bool Delete(int id);
    T GetById(int id);
    T Update(int id, T entity);
}
