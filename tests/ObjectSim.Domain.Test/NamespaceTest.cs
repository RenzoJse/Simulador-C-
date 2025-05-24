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
}
