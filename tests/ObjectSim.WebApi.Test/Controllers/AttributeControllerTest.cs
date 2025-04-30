using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
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

    [TestMethod]
    public void GetAll_ShouldThrowException_WhenNoAttributesExist()
    {
        _attributeServiceMock
            .Setup(service => service.GetAll())
            .Throws(new Exception("No attributes found."));

        Action act = () => _attributeController.GetAll();

        act.Should().Throw<Exception>()
           .WithMessage("No attributes found.");
    }
    [TestMethod]
    public void Create_ValidModel_ShouldReturnCreatedResult()
    {
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "Color",
            Visibility = "Public",
            DataTypeName = "string",
            DataTypeKind = "Reference",
            ClassId = Guid.NewGuid()
        };

        var domainAttribute = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Color",
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = ReferenceType.Create("string"),
            ClassId = modelIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.CreateAttribute(It.IsAny<CreateAttributeArgs>()))
            .Returns(domainAttribute);

        var result = _attributeController.Create(modelIn);

        var created = result as CreatedAtActionResult;
        Assert.IsNotNull(created);

        var outModel = created.Value as AttributeDtoOut;
        Assert.IsNotNull(outModel);
        Assert.AreEqual("Color", outModel.Name);
        Assert.AreEqual("Public", outModel.Visibility);
        Assert.AreEqual("Reference", outModel.DataTypeKind);
        Assert.AreEqual("string", outModel.DataTypeName);

        _attributeServiceMock.Verify(x => x.CreateAttribute(It.IsAny<CreateAttributeArgs>()), Times.Once);
    }
    [TestMethod]
    public void Create_NullModel_ShouldReturnBadRequest()
    {
        var result = _attributeController.Create(null!);
        var badRequest = result as BadRequestResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
    }


}
