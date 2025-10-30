using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateNamespaceArgsTest
{
    [TestMethod]
    public void Constructor_WithValidValues_ShouldSetProperties()
    {
        var name = "MyNamespace";
        Guid? parentId = Guid.NewGuid();
        var classIds = new List<Guid> { Guid.NewGuid() };

        var args = new CreateNamespaceArgs(name, parentId, classIds);

        Assert.AreEqual(name, args.Name);
        Assert.AreEqual(parentId, args.ParentId);
        Assert.AreEqual(classIds, args.ClassIds);
        Assert.AreNotEqual(Guid.Empty, args.Id);
    }

    [TestMethod]
    public void Constructor_WithNullParentId_ShouldAllowRootNamespace()
    {
        var classIds = new List<Guid>();
        var args = new CreateNamespaceArgs("Root", null, classIds);

        Assert.IsNull(args.ParentId);
        Assert.AreEqual(0, args.ClassIds.Count);
    }

    [TestMethod]
    public void Id_ShouldBeOverridable()
    {
        var customId = Guid.NewGuid();
        var classIds = new List<Guid>();
        var args = new CreateNamespaceArgs("Test", null, classIds)
        {
            Id = customId
        };

        Assert.AreEqual(customId, args.Id);
    }
}
