using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

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

    #region GetAll

    #region Success

    [TestMethod]
    public void GetAll_ShouldReturnAttributes_WhenThereAreElements()
    {
        var attributes = new List<Attribute>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Attribute1",
                Visibility = Attribute.AttributeVisibility.Public,
                ClassId = Guid.NewGuid(),
                DataType = new ValueType(Guid.NewGuid(), "int")
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Attribute2",
                Visibility = Attribute.AttributeVisibility.Private,
                ClassId = Guid.NewGuid(),
                DataType = new ValueType(Guid.NewGuid(), "bool")
            }
        };

        _attributeServiceMock
            .Setup(service => service.GetAll())
            .Returns(attributes);

        var result = _attributeController.GetAll() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var returnedAttributes = result.Value as List<AttributeDtoOut>;
        Assert.IsNotNull(returnedAttributes);
        Assert.AreEqual(2, returnedAttributes.Count);
        Assert.AreEqual("Attribute1", returnedAttributes[0].Name);
        Assert.AreEqual("Attribute2", returnedAttributes[1].Name);
    }

    [TestMethod]
    public void GetAll_ShouldReturnEmptyList_WhenNoAttributesExist()
    {
        var emptyAttributes = new List<Attribute>();

        _attributeServiceMock
            .Setup(service => service.GetAll())
            .Returns(emptyAttributes);

        var result = _attributeController.GetAll() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var returnedAttributes = result.Value as List<AttributeDtoOut>;
        Assert.IsNotNull(returnedAttributes);
        Assert.AreEqual(0, returnedAttributes.Count);
    }

    #endregion

    #region Error

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

    #endregion

    #endregion

    #region Create

    #region Error

    [TestMethod]
    public void Create_NullModel_ShouldThrowNullReferenceException()
    {
        Action act = () => _attributeController.Create(null!);
        act.Should().Throw<NullReferenceException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void Create_ValidModel_ShouldReturnCreatedResult()
    {
        var dataType = new ReferenceType(Guid.NewGuid(), "string");

        var modelIn = new CreateAttributeDtoIn
        {
            Name = "Color", Visibility = "Public", DataTypeId = dataType.Id.ToString(), ClassId = Guid.NewGuid()
        };

        var domainAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Color",
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = dataType,
            ClassId = modelIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.CreateAttribute(It.IsAny<CreateAttributeArgs>()))
            .Returns(domainAttribute);

        IActionResult result = _attributeController.Create(modelIn);

        var created = result as CreatedAtActionResult;
        Assert.IsNotNull(created);

        var outModel = created.Value as AttributeDtoOut;
        Assert.IsNotNull(outModel);
        Assert.AreEqual("Color", outModel.Name);
        Assert.AreEqual("Public", outModel.Visibility);

        _attributeServiceMock.Verify(x => x.CreateAttribute(It.IsAny<CreateAttributeArgs>()), Times.Once);
    }

    [TestMethod]
    public void Create_ShouldReturnCreatedAtAction_WithCorrectActionName()
    {
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "TestAttr",
            Visibility = "Public",
            DataTypeId = Guid.NewGuid().ToString(),
            ClassId = Guid.NewGuid()
        };

        var domainAttr = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttr",
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType(Guid.NewGuid(), "string"),
            ClassId = modelIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.CreateAttribute(It.IsAny<CreateAttributeArgs>()))
            .Returns(domainAttr);

        IActionResult result = _attributeController.Create(modelIn);

        var created = result as CreatedAtActionResult;
        Assert.IsNotNull(created);
        Assert.AreEqual("Create", created.ActionName);
        Assert.AreEqual(domainAttr.Id, ((AttributeDtoOut)created.Value!).Id);
    }

    [TestMethod]
    public void Create_ShouldReturn201AndCorrectRouteValues()
    {
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "TestAttr",
            Visibility = "Public",
            DataTypeId = Guid.NewGuid().ToString(),
            ClassId = Guid.NewGuid()
        };

        var domainAttr = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttr",
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType(Guid.NewGuid(), "string"),
            ClassId = modelIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.CreateAttribute(It.IsAny<CreateAttributeArgs>()))
            .Returns(domainAttr);

        var result = _attributeController.Create(modelIn) as CreatedAtActionResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(201, result.StatusCode);
        Assert.IsTrue(result.RouteValues.ContainsKey("id"));
        Assert.AreEqual(domainAttr.Id, result.RouteValues["id"]);
    }

    #endregion

    #endregion

    #region GetById

    #region Success

    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        var id = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = id,
            Name = "Test",
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType(Guid.NewGuid(), "string")
        };

        _attributeServiceMock
            .Setup(s => s.GetById(id))
            .Returns(attribute);

        IActionResult result = _attributeController.GetById(id);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var dto = okResult.Value as AttributeDtoOut;
        Assert.IsNotNull(dto);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetById_ShouldThrowException_WhenServiceFails()
    {
        var id = Guid.NewGuid();

        _attributeServiceMock
            .Setup(s => s.GetById(id))
            .Throws(new Exception("Unexpected error."));

        Action act = () => _attributeController.GetById(id);

        act.Should().Throw<Exception>()
            .WithMessage("Unexpected error.");
    }

    #endregion

    #endregion

    #region GetByClassId

    #region Success

    [TestMethod]
    public void GetByClassId_ValidId_ShouldReturnAttributes()
    {
        var classId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "cantidad",
            Visibility = Attribute.AttributeVisibility.Public,
            ClassId = classId,
            DataType = new ValueType(Guid.NewGuid(), "int")
        };

        var attributes = new List<Attribute> { attribute };

        _attributeServiceMock
            .Setup(s => s.GetByClassId(classId))
            .Returns(attributes);

        IActionResult result = _attributeController.GetByClassId(classId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var dtoList = okResult.Value as List<AttributeDtoOut>;
        Assert.IsNotNull(dtoList);
        Assert.AreEqual(1, dtoList.Count);

        AttributeDtoOut dto = dtoList[0];
        Assert.AreEqual("cantidad", dto.Name);
        Assert.AreEqual("Public", dto.Visibility);
        Assert.AreEqual(classId, dto.ClassId);

        _attributeServiceMock.Verify(s => s.GetByClassId(classId), Times.Once);
    }

    [TestMethod]
    public void GetByClassId_WhenNoAttributesExist_ShouldReturnEmptyList()
    {
        var classId = Guid.NewGuid();
        _attributeServiceMock
            .Setup(s => s.GetByClassId(classId))
            .Returns([]);

        var ok = _attributeController.GetByClassId(classId) as OkObjectResult;
        var list = ok!.Value as List<AttributeDtoOut>;

        Assert.AreEqual(200, ok.StatusCode);
        Assert.IsNotNull(list);
        Assert.AreEqual(0, list.Count);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetByClassId_InvalidId_ShouldReturnBadRequest()
    {
        Guid invalidId = Guid.Empty;

        _attributeServiceMock
            .Setup(s => s.GetByClassId(invalidId))
            .Throws(new ArgumentException("Invalid ClassId"));

        Action act = () => _attributeController.GetByClassId(invalidId);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid ClassId");

        _attributeServiceMock.Verify(s => s.GetByClassId(invalidId), Times.Once);
    }

    #endregion

    #endregion

    #region Delete

    #region Success

    [TestMethod]
    public void Delete_ValidId_ShouldReturnOk()
    {
        var id = Guid.NewGuid();

        _attributeServiceMock
            .Setup(service => service.Delete(id))
            .Returns(true);

        var result = _attributeController.Delete(id) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(true, result.Value);

        _attributeServiceMock.Verify(service => service.Delete(id), Times.Once);
    }

    [TestMethod]
    public void Delete_ServiceReturnsFalse_ShouldReturnOkFalse()
    {
        var id = Guid.NewGuid();
        _attributeServiceMock
            .Setup(s => s.Delete(id))
            .Returns(false);

        var ok = _attributeController.Delete(id) as OkObjectResult;
        Assert.AreEqual(200, ok!.StatusCode);
        Assert.AreEqual(false, ok.Value);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Delete_InvalidId_ShouldThrowException()
    {
        var invalidId = Guid.NewGuid();

        _attributeServiceMock
            .Setup(service => service.Delete(invalidId))
            .Throws(new Exception("Attribute not found."));

        Action act = () => _attributeController.Delete(invalidId);

        act.Should().Throw<Exception>()
            .WithMessage("Attribute not found.");

        _attributeServiceMock.Verify(service => service.Delete(invalidId), Times.Once);
    }

    [TestMethod]
    public void Delete_EmptyGuid_ShouldThrowArgumentException()
    {
        Guid emptyId = Guid.Empty;

        _attributeServiceMock
            .Setup(service => service.Delete(emptyId))
            .Throws(new ArgumentException("Id must not be empty."));

        Action act = () => _attributeController.Delete(emptyId);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must not be empty.");

        _attributeServiceMock.Verify(service => service.Delete(emptyId), Times.Once);
    }

    #endregion

    #endregion

    #region Update

    #region Success

    [TestMethod]
    public void Update_ValidInput_ShouldReturnUpdatedDto()
    {
        var id = Guid.NewGuid();
        var dtoIn = new CreateAttributeDtoIn
        {
            Name = "Color", Visibility = "Public", DataTypeId = Guid.NewGuid().ToString(), ClassId = Guid.NewGuid()
        };

        var updatedAttr = new Attribute
        {
            Id = id,
            Name = "Color",
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType(Guid.NewGuid(), "string"),
            ClassId = dtoIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.Update(id, It.IsAny<CreateAttributeArgs>()))
            .Returns(updatedAttr);

        IActionResult result = _attributeController.Update(id, dtoIn);
        var ok = result as OkObjectResult;

        Assert.IsNotNull(ok);
        Assert.AreEqual(200, ok.StatusCode);

        var dtoOut = ok.Value as AttributeDtoOut;
        Assert.IsNotNull(dtoOut);
        Assert.AreEqual("Color", dtoOut.Name);
        Assert.AreEqual("Public", dtoOut.Visibility);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Update_InvalidId_ShouldThrowException()
    {
        var id = Guid.NewGuid();
        var dtoIn = new CreateAttributeDtoIn
        {
            Name = "Color", Visibility = "Public", DataTypeId = Guid.NewGuid().ToString(), ClassId = Guid.NewGuid()
        };

        _attributeServiceMock
            .Setup(s => s.Update(id, It.IsAny<CreateAttributeArgs>()))
            .Throws(new KeyNotFoundException("Attribute not found."));

        Action act = () => _attributeController.Update(id, dtoIn);

        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("Attribute not found.");
    }

    [TestMethod]
    public void Update_NullModel_ShouldThrowNullReferenceException()
    {
        var id = Guid.NewGuid();

        Action act = () => _attributeController.Update(id, null!);

        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void Update_EmptyGuid_ShouldThrowArgumentException()
    {
        var dtoIn = new CreateAttributeDtoIn
        {
            Name = "Something",
            Visibility = "Public",
            DataTypeId = Guid.NewGuid().ToString(),
            ClassId = Guid.NewGuid()
        };

        Guid emptyId = Guid.Empty;

        _attributeServiceMock
            .Setup(s => s.Update(emptyId, It.IsAny<CreateAttributeArgs>()))
            .Throws(new ArgumentException("Id must not be empty."));

        Action act = () => _attributeController.Update(emptyId, dtoIn);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must not be empty.");
    }

    #endregion

    #endregion
}
