using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class LocalVariableTest
{
    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var variable = new LocalVariable();
        variable.Name = "myVar";
        variable.Name.Should().Be("myVar");
    }

    [TestMethod]
    public void Type_Property_SetAndGet_ShouldBeEqual()
    {
        var variable = new LocalVariable();
        variable.Type = LocalVariable.LocalVariableDataType.Decimal;
        variable.Type.Should().Be(LocalVariable.LocalVariableDataType.Decimal);
    }

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var variable = new LocalVariable { Id = id };
        variable.Id.Should().Be(id);
    }

    [TestMethod]
    public void SetName_WhenNull_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        Action act = () => variable.Name = null!;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenEmpty_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        Action act = () => variable.Name = "";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenWhitespace_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        Action act = () => variable.Name = "   ";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenStartsWithNumber_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        Action act = () => variable.Name = "9variable";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void SetName_WhenTooLong_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        var longName = new string('a', 101);
        Action act = () => variable.Name = longName;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot exceed 100 characters.");
    }

    [TestMethod]
    public void SetType_WhenInvalidEnum_ShouldThrowArgumentException()
    {
        var variable = new LocalVariable();
        Action act = () => variable.Type = (LocalVariable.LocalVariableDataType)999;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid data type.");
    }
}
