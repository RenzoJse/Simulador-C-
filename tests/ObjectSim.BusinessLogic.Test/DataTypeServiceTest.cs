using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class DataTypeServiceTest
{
    private Mock<IRepository<Class>> _classRepositoryMock = null!;
    private DataTypeService _dataTypeServiceTest = null!;
    private Mock<IRepository<DataType>> _dataTypeRepositoryMock = null!;

    private readonly Guid _valueTypeId = Guid.NewGuid();

    [TestInitialize]
    public void Setup()
    {
        _classRepositoryMock = new Mock<IRepository<Class>>();
        _dataTypeRepositoryMock = new Mock<IRepository<DataType>>();
        _dataTypeServiceTest = new DataTypeService(_classRepositoryMock.Object, _dataTypeRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _classRepositoryMock.VerifyAll();
    }

    #region CreateDataType

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateDataType_ObjectReferenceTypeWithoutClass_ShouldThrow()
    {
        var args = new CreateDataTypeArgs("object", "Reference");
        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([]);

        _dataTypeServiceTest.CreateDataType(args);
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateDataType_ReferenceType_ObjectWithRegisteredClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "object" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
        Assert.AreEqual("Reference", result.Type);
    }

    [TestMethod]
    public void CreateDataType_ObjectReferenceTypeWithClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "object" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
    }

    [TestMethod]
    public void CreateDataType_WithValidIntType_ShouldCreateValueType()
    {
        var args = new CreateDataTypeArgs("MyInt", "int");

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ValueType));
        Assert.AreEqual("MyInt", result.Name);
        Assert.AreEqual("int", result.Type);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateDataType_ShouldThrow_WhenReferenceTypeNotFoundInClasses()
    {
        var args = new CreateDataTypeArgs("InvalidRef", "UnknownClass");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([]);

        _dataTypeServiceTest.CreateDataType(args);
    }

    [TestMethod]
    public void CreateDataType_ShouldReturnReferenceType_WhenTypeMatchesExistingClass()
    {
        var args = new CreateDataTypeArgs("MyClientRef", "Client");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "Client" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("MyClientRef", result.Name);
        Assert.AreEqual("Client", result.Type);
    }

    [TestMethod]
    public void CreateDataType_WithStringType_ShouldCreateReferenceType()
    {
        var args = new CreateDataTypeArgs("MyString", "string");

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("MyString", result.Name);
        Assert.AreEqual("string", result.Type);
    }

    #endregion

    #endregion

    [TestMethod]
    public void GetById_WhenValueTypeExists_ReturnsValueType()
    {
        var vt = new ValueType { Id = _valueTypeId, Name = "int", Type = "int" };
        _dataTypeRepositoryMock
            .Setup(r => r.Get(It.Is<Func<DataType, bool>>(f => f(vt))))
            .Returns(vt);

        var result = _dataTypeServiceTest.GetById(_valueTypeId);

        Assert.IsInstanceOfType(result, typeof(ValueType));
        Assert.AreEqual(vt, result);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetById_WhenNotFound_Throws()
    {
        _dataTypeRepositoryMock.Setup(r => r.Get(It.IsAny<Func<DataType, bool>>()))
        .Returns((DataType?)null);

        _dataTypeServiceTest.GetById(new Guid());
    }

    #region GetAll

    [TestMethod]
    public void GetAll_ShouldReturnListOfDataTypes()
    {
        var list = new List<DataType>
        {
            new ValueType("int", "int"),
            new ReferenceType("str", "string")
        };

        _dataTypeRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<DataType, bool>>()))
            .Returns(list);

        var result = _dataTypeServiceTest.GetAll();

        CollectionAssert.AreEquivalent(list, result);
    }

    #endregion
}
