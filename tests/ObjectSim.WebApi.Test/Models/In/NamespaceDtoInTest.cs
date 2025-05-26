

using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;
[TestClass]
public class NamespaceDtoInTest
{
    [TestMethod]
    public void ToArgs_ShouldReturnCreateNamespaceArgsWithSameValues()
    {
        var dto = new CreateNamespaceDtoIn
        {
            Name = "MyNamespace",
            ParentId = Guid.NewGuid()
        };

        var args = dto.ToArgs();

        Assert.AreEqual(dto.Name, args.Name);
        Assert.AreEqual(dto.ParentId, args.ParentId);
        Assert.AreNotEqual(Guid.Empty, args.Id);
    }
    [TestMethod]
    public void ToArgs_WhenParentIdIsNull_ShouldAllowRootNamespace()
    {
        var dto = new CreateNamespaceDtoIn { Name = "Root", ParentId = null };

        var args = dto.ToArgs();

        Assert.IsNull(args.ParentId);
    }
}
