namespace ObjectSim.Domain;
public class Namespace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
    public List<Namespace> Children { get; set; } = [];
    public List<Class> Classes { get; set; } = [];
}
