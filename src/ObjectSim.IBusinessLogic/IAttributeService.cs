namespace ObjectSim.IBusinessLogic;
public interface IAttributeService<T>
{
    T Create(T attribute);
    List<T> GetAll();
    bool Delete(Guid id);
}
