namespace ObjectSim.Domain.Test;
[TestClass]
public class NamespaceTest
{
    [TestMethod]
    public void AddChild_ShouldAddChildAndSetParentId()
    {
        // Arrange
        var parent = new Namespace { Name = "Root" };
        var child = new Namespace { Name = "Child" };

        // Act
        parent.AddChild(child);

        // Assert
        Assert.AreEqual(parent.Id, child.ParentId);
        Assert.IsTrue(parent.Children.Contains(child));
    }
}
