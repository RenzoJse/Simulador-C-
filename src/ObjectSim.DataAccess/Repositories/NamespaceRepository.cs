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
        var allNamespaces = _context.Namespaces.AsNoTracking().ToList();

        var root = allNamespaces.FirstOrDefault(n => n.Id == id);

        if(root != null)
        {
            BuildHierarchy(root, allNamespaces);
        }

        return root;
    }

    private void BuildHierarchy(Namespace parent, List<Namespace> allNamespaces)
    {
        var children = allNamespaces
            .Where(n => n.ParentId == parent.Id)
            .ToList();

        parent.Children = children;

        foreach(var child in children)
        {
            BuildHierarchy(child, allNamespaces);
        }
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
