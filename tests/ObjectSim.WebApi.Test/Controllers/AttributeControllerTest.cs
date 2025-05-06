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
            DataTypeName = "myString",
            DataTypeKind = "string",
            ClassId = Guid.NewGuid()
        };

        var domainAttribute = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Color",
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType("myString", "string", []),
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
        Assert.AreEqual("string", outModel.DataTypeKind);
        Assert.AreEqual("myString", outModel.DataTypeName);

        _attributeServiceMock.Verify(x => x.CreateAttribute(It.IsAny<CreateAttributeArgs>()), Times.Once);
    }

    [TestMethod]
    public void Create_NullModel_ShouldReturnBadRequest()
    {
        var invalidId = Guid.NewGuid();

        _attributeServiceMock
            .Setup(s => s.GetByClassId(invalidId))
            .Throws(new ArgumentException("Invalid ClassId"));

        Action act = () => _attributeController.GetByClassId(invalidId);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid ClassId");
    }

    [TestMethod]
    public void Create_ShouldReturnCreatedAtAction_WithCorrectActionName()
    {
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "TestAttr",
            Visibility = "Public",
            DataTypeName = "string",
            DataTypeKind = "Reference",
            ClassId = Guid.NewGuid()
        };

        var domainAttr = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttr",
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType("myString", "string", []),
            ClassId = modelIn.ClassId
        };

        _attributeServiceMock
            .Setup(s => s.CreateAttribute(It.IsAny<CreateAttributeArgs>()))
            .Returns(domainAttr);

        var result = _attributeController.Create(modelIn);

        var created = result as CreatedAtActionResult;
        Assert.IsNotNull(created);
        Assert.AreEqual("Create", created.ActionName);
        Assert.AreEqual(domainAttr.Id, ((AttributeDtoOut)created.Value!).Id);
    }
    /*
    [TestMethod]
    public void Update_ValidModel_ShouldReturnUpdatedAttribute()
    {
        var id = Guid.NewGuid();
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "Size",
            Visibility = "Public",
            DataTypeName = "int",
            DataTypeKind = "Value",
            ClassId = Guid.NewGuid()
        };

        var updatedAttribute = new Domain.Attribute
        {
            Id = id,
            Name = modelIn.Name,
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            ClassId = modelIn.ClassId,
            DataType = new ReferenceType("myString", "string", []),
        };

        _attributeServiceMock
            .Setup(s => s.Update(id, It.Is<Domain.Attribute>(a =>
                a.Name == modelIn.Name &&
                a.ClassId == modelIn.ClassId &&
                a.Visibility == Domain.Attribute.AttributeVisibility.Public &&
                a.DataType.Name == modelIn.DataTypeName)))
            .Returns(updatedAttribute);

        var result = _attributeController.Update(id, modelIn);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var dtoOut = okResult.Value as AttributeDtoOut;
        Assert.IsNotNull(dtoOut);
        Assert.AreEqual("Size", dtoOut.Name);
        Assert.AreEqual("Public", dtoOut.Visibility);
        _attributeServiceMock.Verify(x => x.Update(id, It.IsAny<Domain.Attribute>()), Times.Once);
    }

    [TestMethod]
    public void Update_InvalidModel_ShouldReturnBadRequest()
    {
        var id = Guid.NewGuid();
        var modelIn = new CreateAttributeDtoIn
        {
            Name = "",
            Visibility = "Public",
            DataTypeName = "int",
            DataTypeKind = "Value",
            ClassId = Guid.NewGuid()
        };

        _attributeController.ModelState.AddModelError("Name", "Name is required.");

        var result = _attributeController.Update(id, modelIn);

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
    }
       */
    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        var id = Guid.NewGuid();
        var attribute = new Domain.Attribute
        {
            Id = id,
            Name = "Test",
            ClassId = Guid.NewGuid(),
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType("myString", "string", [])
        };

        _attributeServiceMock
            .Setup(s => s.GetById(id))
            .Returns(attribute);

        var result = _attributeController.GetById(id);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var dto = okResult.Value as AttributeDtoOut;
        Assert.IsNotNull(dto);
    }

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

    [TestMethod]
    public void GetByClassId_ValidId_ShouldReturnAttributes()
    {
        var classId = Guid.NewGuid();
        var attribute = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "cantidad",
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            ClassId = classId,
            DataType = new Domain.ValueType("cantidad", "int", [])
        };

        var attributes = new List<Domain.Attribute> { attribute };

        _attributeServiceMock
            .Setup(s => s.GetByClassId(classId))
            .Returns(attributes);

        var result = _attributeController.GetByClassId(classId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var dtoList = okResult.Value as List<AttributeDtoOut>;
        Assert.IsNotNull(dtoList);
        Assert.AreEqual(1, dtoList.Count);

        var dto = dtoList[0];
        Assert.AreEqual("cantidad", dto.Name);
        Assert.AreEqual("Public", dto.Visibility);
        Assert.AreEqual("int", dto.DataTypeKind);
        Assert.AreEqual("cantidad", dto.DataTypeName);
        Assert.AreEqual(classId, dto.ClassId);

        _attributeServiceMock.Verify(s => s.GetByClassId(classId), Times.Once);
    }


    [TestMethod]
    public void GetByClassId_InvalidId_ShouldReturnBadRequest()
    {
        var invalidId = Guid.Empty;

        _attributeServiceMock
            .Setup(s => s.GetByClassId(invalidId))
            .Throws(new ArgumentException("Invalid ClassId"));

        Action act = () => _attributeController.GetByClassId(invalidId);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid ClassId");

        _attributeServiceMock.Verify(s => s.GetByClassId(invalidId), Times.Once);
    }
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
    public void Delete_InvalidId_ShouldThrowException()
    {
        var invalidId = Guid.NewGuid();

        _attributeServiceMock
            .Setup(service => service.Delete(invalidId))
            .Throws(new Exception("Attribute cannot be null."));

        Action act = () => _attributeController.Delete(invalidId);

        act.Should().Throw<Exception>()
           .WithMessage("Attribute cannot be null.");

        _attributeServiceMock.Verify(service => service.Delete(invalidId), Times.Once);
    }



}
