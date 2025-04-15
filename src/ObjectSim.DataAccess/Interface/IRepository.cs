using System.Linq.Expressions;

namespace ObjectSim.DataAccess.Interface;

public interface IRepository<T>
{
    T Add(T element);
    T Update(T element);
    T? Get(Func<T, bool> filter);
    List<T> GetAll(Func<T, bool> filter);
    bool Exists(Expression<Func<T, bool>> filter);
    void Delete(T element);
}
