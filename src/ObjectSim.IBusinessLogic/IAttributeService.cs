namespace ObjectSim.IBusinessLogic;
public interface IAttributeService<T>
{
    T Create(T attribute);
    List<T> GetAll();
}
