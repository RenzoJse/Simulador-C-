using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.IBusinessLogic;
namespace ObjectSim.WebApi.Test.Controllers;
[TestClass]
public class AttributeControllerTest
{
    private Mock<IAttributeService> _attributeServiceMock = null!;
    private AttributeController _attributeController = null!;
    private Mock<IRepository<Domain.Attribute>> _attributeRepositoryMock = null!;


    private static readonly Domain.Attribute TestAttribute = new Domain.Attribute
    {
        Name = "TestAttribute",
    };
    [TestInitialize]
    public void Setup()
    {
        _attribureServiceMock = new Mock<IAttributeService>();
        _attributeController = new AttributeController(_attributeServiceMock.Object);
        SetupHttpContext();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attributeServiceMock.VerifyAll();
    }
    [TestMethod]
    public void GetAll_ShouldReturnAttributes_WhenThereAreElements()
    {
        var attributes = new List<Domain.Attribute>
        {
            new Domain.Attribute { Name = "Attribute1" },
            new Domain.Attribute { Name = "Attribute2" }
        };

        _attributeRepositoryMock.Setup(x => x.GetAll()).Returns(attributes);

        var result = _attributeController.GetAll() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        var userList = result.Value as List<Domain.Attribute>;
        Assert.IsNotNull(userList);
        Assert.AreEqual(2, userList.Count);
    }
}
