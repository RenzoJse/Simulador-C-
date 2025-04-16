namespace ObjectSim.IBusinessLogic;

public interface IParameterService<T>
{
    T Create(T Entity);
}
