namespace ObjectSim.Domain;
public class Namespace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
    public List<Namespace> Children { get; set; } = [];
    public List<Class> Classes { get; set; } = [];

    public void AddChild(Namespace child)
    {
        ArgumentNullException.ThrowIfNull(child);

        child.ParentId = Id;
        Children.Add(child);
    }
    public void RemoveChild(Namespace child)
    {
        ArgumentNullException.ThrowIfNull(child);

        Children.Remove(child);
    }
    public IEnumerable<Namespace> GetAllDescendants()
    {
        foreach(var child in Children)
        {
            yield return child;

            foreach(var descendant in child.GetAllDescendants())
            {
                yield return descendant;
            }
        }
    }
    public void Validate()
    {
        ValidateId(Id);
    }
    private static void ValidateId(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id must be a valid non-empty GUID.");
        }
    }
}
