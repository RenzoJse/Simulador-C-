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

        var args = new CreateNamespaceArgs(name, parentId);

        Assert.AreEqual(name, args.Name);
        Assert.AreEqual(parentId, args.ParentId);
        Assert.AreNotEqual(Guid.Empty, args.Id);
    }
}
