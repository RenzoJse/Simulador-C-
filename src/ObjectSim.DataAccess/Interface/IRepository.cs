namespace ObjectSim.DataAccess.Interface;

public interface IRepository<T>
{
    T Add(T element);
}
