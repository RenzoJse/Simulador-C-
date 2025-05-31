namespace ObjectSim.Domain.Test;
[TestClass]
public class NamespaceTest
{
    [TestMethod]
    public void AddChild_ShouldAddChildAndSetParentId()
    {
        var parent = new Namespace { Name = "Root" };
        var child = new Namespace { Name = "Child" };

        parent.AddChild(child);

        Assert.AreEqual(parent.Id, child.ParentId);
        Assert.IsTrue(parent.Children.Contains(child));
    }
    [TestMethod]
    public void RemoveChild_ShouldRemoveChild()
    {
        var parent = new Namespace { Name = "Root" };
        var child = new Namespace { Name = "Child" };
        parent.AddChild(child);

        parent.RemoveChild(child);

        Assert.IsFalse(parent.Children.Contains(child));
    }
    [TestMethod]
    public void GetAllDescendants_ShouldReturnAllNestedChildren()
    {
        var root = new Namespace { Name = "Root" };
        var child1 = new Namespace { Name = "Child1" };
        var child2 = new Namespace { Name = "Child2" };
        var grandChild = new Namespace { Name = "GrandChild" };

        child1.AddChild(grandChild);
        root.AddChild(child1);
        root.AddChild(child2);

        var descendants = root.GetAllDescendants().ToList();

        Assert.AreEqual(3, descendants.Count);
        CollectionAssert.Contains(descendants, child1);
        CollectionAssert.Contains(descendants, child2);
        CollectionAssert.Contains(descendants, grandChild);
    }
    [TestMethod]
    public void Validate_WithValidNamespace_ShouldNotThrow()
    {
        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "Core"
        };

        ns.Validate();
    }
    [TestMethod]
    public void Validate_WhenNameTooLong_ShouldThrow()
    {
        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "ThisIsTooLong"
        };

        Assert.ThrowsException<ArgumentException>(ns.Validate, "Name cannot be less than 1 or more than 10 characters.");
    }
    [TestMethod]
    public void Validate_WhenNameTooShort_ShouldThrow()
    {
        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = ""
        };

        Assert.ThrowsException<ArgumentException>(ns.Validate, "Name cannot be null or whitespace.");
    }
}
