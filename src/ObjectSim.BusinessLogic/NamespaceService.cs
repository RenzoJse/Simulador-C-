

using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class NamespaceService(INamespaceRepository repository, IClassService classService) : INamespaceService
{
    private readonly INamespaceRepository _repository = repository;
    private readonly IClassService _classService = classService;

    public Namespace Create(CreateNamespaceArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var ns = new Namespace
        {
            Id = args.Id,
            Name = args.Name,
            ParentId = args.ParentId
        };
        if(args.ClassIds.Any())
        {
            var classes = args.ClassIds
                .Select(id => _classService.GetById((Guid?)id))
                .ToList();

            var existingIds = ns.Classes.Select(c => c.Id).ToHashSet();

            foreach(var cls in classes)
            {
                if(!existingIds.Contains(cls.Id))
                {
                    ns.Classes.Add(cls);
                }
            }
        }

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
