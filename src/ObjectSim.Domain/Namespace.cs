using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ObjectSim.Domain;
public class Namespace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
    public List<Namespace> Children { get; set; } = [];
    [NotMapped]
    public List<Class> Classes { get; set; } = [];
    public string ClassIdsSerialized { get; set; } = string.Empty;

    public List<Guid> ClassIds
    {
        get => string.IsNullOrWhiteSpace(ClassIdsSerialized)
            ? []
            : ClassIdsSerialized.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                .Select(Guid.Parse).ToList();

        set => ClassIdsSerialized = value is null || value.Count == 0
            ? string.Empty
            : string.Join(';', value);
    }
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
        ValidateName(Name);
    }
    private static void ValidateId(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id must be a valid non-empty GUID.");
        }
    }
    private static void ValidateName(string? name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.");
        }

        if(name.Length > 10 || name.Length < 1)
        {
            throw new ArgumentException("Name cannot be less than 1 or more than 10 characters.");
        }

        if(Regex.IsMatch(name, @"^\d"))
        {
            throw new ArgumentException("Name cannot be null or start with a num.");
        }
    }
}
