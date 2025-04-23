using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ParameterTest
{
    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var parameter = new Parameter();
        parameter.Name = "myParam";
        parameter.Name.Should().Be("myParam");
    }

    [TestMethod]
    public void Type_Property_SetAndGet_ShouldBeEqual()
    {
        var parameter = new Parameter();
        parameter.Type = Parameter.ParameterDataType.Int;
        parameter.Type.Should().Be(Parameter.ParameterDataType.Int);
    }

    [TestMethod]
    public void Id_ShouldHaveDefaultGuid()
    {
        var parameter = new Parameter();
        parameter.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    public void SetName_WhenNull_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        Action act = () => parameter.Name = null!;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenEmpty_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        Action act = () => parameter.Name = "";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenWhitespace_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        Action act = () => parameter.Name = "   ";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void SetName_WhenStartsWithNumber_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        Action act = () => parameter.Name = "1invalid";
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void SetName_WhenTooLong_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        var longName = new string('p', 101);
        Action act = () => parameter.Name = longName;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Name cannot exceed 100 characters.");
    }

    [TestMethod]
    public void SetType_WhenInvalidEnum_ShouldThrowArgumentException()
    {
        var parameter = new Parameter();
        Action act = () => parameter.Type = (Parameter.ParameterDataType)999;
        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid data type.");
    }
}
