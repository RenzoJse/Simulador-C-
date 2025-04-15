namespace ObjectSim.DataAccess.Test;

[TestClass]
public class RepositoryTest
{
    private readonly DbContext _context = ContextConstructorDB.CreateMemoryContext();
    private readonly Repository<TestEntity> _repository;

    public RepositoryTest()
    {
        _repository = new Repository<TestEntity>(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    #region Add

    [TestMethod]
    public void Add_WhenElementIsProvided_AddsElementToRepository()
    {
        var element = new TestEntity("Test");

        var result = _repository.Add(element);

        result.Should().Be(element);
    }

    #endregion
}

internal sealed record class TestEntity()
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string Nombre { get; set; } = null!;

    public TestEntity(string nombre)
        : this()
    {
        Nombre = nombre;
    }
}
