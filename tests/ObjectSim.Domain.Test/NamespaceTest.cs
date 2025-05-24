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
}
