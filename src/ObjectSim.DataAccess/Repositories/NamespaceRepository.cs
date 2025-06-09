using Microsoft.EntityFrameworkCore;
using ObjectSim.Domain;
using ObjectSim.DataAccess.Interface;

namespace ObjectSim.DataAccess.Repositories;
public class NamespaceRepository(DataContext context):INamespaceRepository
{
    private readonly DataContext _context = context;
    public Namespace Add(Namespace ns)
    {
        _context.Namespaces.Add(ns);
        _context.SaveChanges();
        return ns;
    }

    public List<Namespace> GetAll()
    {
        return _context.Namespaces.ToList();
    }
    public Namespace? GetByIdWithChildren(Guid id)
    {
        var root = _context.Namespaces
            .Include(n => n.Children)
            .FirstOrDefault(n => n.Id == id);

        if(root != null)
        {
            LoadChildrenRecursively(root);
        }

        return root;
    }

    private void LoadChildrenRecursively(Namespace parent)
    {
        foreach(var child in parent.Children)
        {
            _context.Entry(child).Collection(c => c.Children).Load();
            LoadChildrenRecursively(child);
        }
    }
}
