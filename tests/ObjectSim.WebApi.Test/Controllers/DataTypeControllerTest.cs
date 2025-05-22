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
    [TestMethod]
    public void GetAll_WhenCalled_ReturnsOkWithDtoList()
    {
        var dataTypes = new List<DataType>
    {
        new Domain.ValueType("int", "int", []) { Id = Guid.NewGuid() },
        new ReferenceType("str", "string", []) { Id = Guid.NewGuid() }
    };

        var expectedDtos = dataTypes
            .Select(DataTypeInformationDtoOut.ToInfo)
            .ToList();

        _dataTypeServiceMock
            .Setup(s => s.GetAll())
            .Returns(dataTypes);

        var result = _controller.GetAll();

        var okResult = result.Result as OkObjectResult;
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
        var dataType = new Domain.ValueType("int", "int", []) { Id = id, MethodIds = [Guid.NewGuid()] };
        var expectedDto = DataTypeInformationDtoOut.ToInfo(dataType);

        _dataTypeServiceMock
            .Setup(s => s.GetById(id))
            .Returns(dataType);

        var result = _controller.GetById(id);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var actualDto = okResult.Value as DataTypeInformationDtoOut;
        Assert.IsNotNull(actualDto);
        Assert.AreEqual(expectedDto, actualDto);
    }
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

}
