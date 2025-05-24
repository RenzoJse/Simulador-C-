

using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class NamespaceService(IRepository<Namespace> repository) : INamespaceService
{
    private readonly IRepository<Namespace> _repository = repository;

    public Namespace Create(CreateNamespaceArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var ns = new Namespace
        {
            Id = args.Id,
            Name = args.Name,
            ParentId = args.ParentId
        };

        return _repository.Add(ns);
    }
}
