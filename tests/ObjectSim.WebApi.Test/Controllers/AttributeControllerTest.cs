using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
namespace ObjectSim.WebApi.Test.Controllers;
[TestClass]
public class AttributeControllerTest
{
    private Mock<IAttributeService> _attributeServiceMock = null!;
    private AttributeController _attributeController = null!;

    [TestInitialize]
    public void Setup()
    {
        _attributeServiceMock = new Mock<IAttributeService>();
        _attributeController = new AttributeController(_attributeServiceMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attributeServiceMock.VerifyAll();
    }
    [TestMethod]
    public void GetAll_ShouldReturnAttributes_WhenThereAreElements()
    {
        var attributes = new List<ObjectSim.Domain.Attribute>
            {
                new ObjectSim.Domain.Attribute { Name = "Attribute1" },
                new ObjectSim.Domain.Attribute { Name = "Attribute2" }
            };

        _attributeServiceMock
            .Setup(service => service.GetAll())
            .Returns(attributes);

        var result = _attributeController.GetAll() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var returnedAttributes = result.Value as List<ObjectSim.Domain.Attribute>;
        Assert.IsNotNull(returnedAttributes);
        Assert.AreEqual(2, returnedAttributes.Count);
        Assert.AreEqual("Attribute1", returnedAttributes[0].Name);
        Assert.AreEqual("Attribute2", returnedAttributes[1].Name);
    }
    [TestMethod]
    public void GetAll_ShouldReturnEmptyList_WhenNoAttributesExist()
    {
        var emptyAttributes = new List<ObjectSim.Domain.Attribute>();

        _attributeServiceMock
            .Setup(service => service.GetAll())
            .Returns(emptyAttributes);

        var result = _attributeController.GetAll() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var returnedAttributes = result.Value as List<ObjectSim.Domain.Attribute>;
        Assert.IsNotNull(returnedAttributes);
        Assert.AreEqual(0, returnedAttributes.Count);
    }

}
