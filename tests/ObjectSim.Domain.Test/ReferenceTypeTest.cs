using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ReferenceTypeTest
{
    private static readonly List<Guid> methodsIds = [];

    #region CreateReferenceType

    #region Error

    [TestMethod]
    public void CreateReferenceType_WhenMethodsIdsAreNull_ThrowsArgumentNullException()
    {
        const string name = "myString";
        const string type = "string";

        Assert.ThrowsException<ArgumentNullException>(() => new ReferenceType(name, type, null!));
    }

    [TestMethod]
    public void CreateReferenceType_WhenNameIsNullOrEmpty_ShouldThrowArgumentException()
    {
        string? invalidName = null;
        const string type = "string";

        Action action = () =>
        {
            var referenceType = new ReferenceType(invalidName, type, methodsIds);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or empty.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateReferenceType_ValidInput_CreatesReferenceType()
    {
        const string name = "myString";
        const string type = "string";

        var referenceType = new ReferenceType(name, type, methodsIds);

        Assert.IsNotNull(referenceType);
        Assert.AreEqual(name, referenceType.Name);
        Assert.AreEqual(type, referenceType.Type);
    }

    [TestMethod]
    public void PrivateConstructor_ValidInput_CreatesReferenceType()
    {
        var constructor = typeof(ReferenceType).GetConstructor( // Reflection to access the private constructor.
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null);

        var referenceType = (ReferenceType)constructor!.Invoke(null);

        referenceType.Id.Should().NotBeEmpty();
        referenceType.Name.Should().BeEmpty();
        referenceType.Type.Should().BeEmpty();
        referenceType.MethodIds.Should().NotBeNull();
        referenceType.MethodIds.Should().BeEmpty();
    }

    #endregion

    #endregion
}
