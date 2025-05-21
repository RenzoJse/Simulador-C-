using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.WebApi.Test.Controllers;
[TestClass]
public class DataTypeControllerTest
{
    private Mock<IDataTypeService> _dataTypeServiceMock = null!;
    private DataTypeController _controller = null!;
    private readonly Guid _existingId = Guid.NewGuid();

    [TestInitialize]
    public void Setup()
    {
        _dataTypeServiceMock = new Mock<IDataTypeService>();
        _controller = new DataTypeController(_dataTypeServiceMock.Object);
    }
    [TestMethod]
    public void GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        _dataTypeServiceMock
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Throws(new BusinessDataException("DataType not found"));

        // Act
        var result = _controller.GetById(Guid.NewGuid());

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual("DataType not found", notFoundResult.Value);
    }

    [TestMethod]
    public void GetAll_WhenCalled_ReturnsOkWithList()
    {
        // Arrange
        var dataTypes = new List<DataType>
        {
            new ValueType("int", "int", []),
            new ReferenceType("str", "string", [])
        };

        _dataTypeServiceMock
            .Setup(s => s.GetAll())
            .Returns(dataTypes);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        CollectionAssert.AreEqual(dataTypes, (List<DataType>)okResult.Value!);
    }
}
