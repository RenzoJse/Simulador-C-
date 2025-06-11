using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ReferenceTypeTest
{
    #region CreateReferenceType

    #region Error

    [TestMethod]
    public void CreateReferenceType_WhenTypeIsNullOrEmpty_ShouldThrowArgumentException()
    {
        Action action = () =>
        {
            var referenceType = new ReferenceType(Guid.NewGuid(), null!);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("ClassId or type cannot be empty or null.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateReferenceType_ValidInput_CreatesReferenceType()
    {
        const string type = "string";

        var referenceType = new ReferenceType(Guid.NewGuid(), type);

        Assert.IsNotNull(referenceType);
        Assert.AreEqual(type, referenceType.Type);
    }

    [TestMethod]
    public void PrivateConstructor_ValidInput_CreatesReferenceType()
    {
        var constructor = typeof(ReferenceType).GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);

        var referenceType = (ReferenceType)constructor!.Invoke(null);

        referenceType.Id.Should().NotBeEmpty();
        referenceType.Type.Should().BeEmpty();
    }

    #endregion

    #endregion
}
