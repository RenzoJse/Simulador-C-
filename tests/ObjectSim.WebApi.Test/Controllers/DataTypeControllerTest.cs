using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Controllers;
[TestClass]
public class DataTypeControllerTest
{
    private Mock<IDataTypeService> _dataTypeServiceMock = null!;
    private DataTypeController _controller = null!;


    [TestInitialize]
    public void Setup()
    {
        _dataTypeServiceMock = new Mock<IDataTypeService>();
        _controller = new DataTypeController(_dataTypeServiceMock.Object);
    }

    #region GetAll
    [TestMethod]
    public void GetAll_WhenCalled_ReturnsOkWithDtoList()
    {
        var dataTypes = new List<DataType>
    {
        new Domain.ValueType(Guid.NewGuid(),"int") { Id = Guid.NewGuid() },
        new ReferenceType(Guid.NewGuid(),"string") { Id = Guid.NewGuid() }
    };

        var expectedDtos = dataTypes
            .Select(DataTypeInformationDtoOut.ToInfo)
            .ToList();

        _dataTypeServiceMock
            .Setup(s => s.GetAll())
            .Returns(dataTypes);

        var result = _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var actualDtos = okResult.Value as List<DataTypeInformationDtoOut>;
        Assert.IsNotNull(actualDtos);
        CollectionAssert.AreEqual(expectedDtos, actualDtos);
    }
    [TestMethod]
    public void GetById_WhenExists_ReturnsOkWithDto()
    {
        var id = Guid.NewGuid();
        var dataType = new Domain.ValueType(Guid.NewGuid(), "int") { Id = id };
        var expectedDto = DataTypeInformationDtoOut.ToInfo(dataType);

        _dataTypeServiceMock
            .Setup(s => s.GetById(id))
            .Returns(dataType);

        var result = _controller.GetById(id);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var actualDto = okResult.Value as DataTypeInformationDtoOut;
        Assert.IsNotNull(actualDto);
        Assert.AreEqual(expectedDto, actualDto);
    }

    [TestMethod]
    public void GetAll_WhenNoDataTypesExist_ReturnsEmptyList()
    {
        _dataTypeServiceMock
            .Setup(s => s.GetAll())
            .Returns(new List<DataType>());

        var result = _controller.GetAll() as OkObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var list = result.Value as List<DataTypeInformationDtoOut>;
        Assert.IsNotNull(list);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void GetAll_ServiceThrowsException_ShouldPropagateException()
    {
        _dataTypeServiceMock
            .Setup(s => s.GetAll())
            .Throws(new InvalidOperationException("Internal error"));

        Action act = () => _controller.GetAll();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Internal error");
    }

    #endregion

    #region GetById
    [TestMethod]
    public void GetById_WhenNotFound_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();

        _dataTypeServiceMock
            .Setup(s => s.GetById(id))
            .Throws(new KeyNotFoundException("DataType not found"));

        Action act = () => _controller.GetById(id);
        Assert.ThrowsException<KeyNotFoundException>(act);
    }

    [TestMethod]
    public void GetById_EmptyGuid_ShouldThrowArgumentException()
    {
        var empty = Guid.Empty;
        _dataTypeServiceMock
            .Setup(s => s.GetById(empty))
            .Throws(new ArgumentException("Empty id"));

        Action act = () => _controller.GetById(empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Empty id");
    }

    [TestMethod]
    public void GetById_ServiceThrowsUnexpectedException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        _dataTypeServiceMock
            .Setup(s => s.GetById(id))
            .Throws(new Exception("Error"));

        Action act = () => _controller.GetById(id);
        act.Should().Throw<Exception>()
           .WithMessage("Error");
    }
    #endregion

}
