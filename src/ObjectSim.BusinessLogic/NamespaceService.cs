

using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class NamespaceService(INamespaceRepository repository) : INamespaceService
{
    private readonly INamespaceRepository _repository = repository;

    public Namespace Create(CreateNamespaceArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var ns = new Namespace
        {
            Id = args.Id,
            Name = args.Name,
            ParentId = args.ParentId,
            ClassIds = args.ClassIds
        };


        return _repository.Add(ns);
    }

    public List<Namespace> GetAll()
    {
        return _repository.GetAll();
    }
    public IEnumerable<Namespace> GetAllDescendants(Guid id)
    {
        var root = _repository.GetByIdWithChildren(id)
                   ?? throw new ArgumentException("Namespace not found");

        var all = root.GetAllDescendants().ToList();
        Console.WriteLine($"Descendants found: {all.Count}");

        return root.GetAllDescendants();
    }
}
