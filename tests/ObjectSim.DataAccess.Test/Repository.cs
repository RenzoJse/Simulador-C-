using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;

namespace ObjectSim.DataAccess.Test;

[TestClass]
public class RepositoryTest
{
    private readonly DbContext _context = ContextConstructorDb.CreateMemoryContext();
    private readonly Repository<TestEntity> _repository;
    private readonly TestEntity _testEntity = new TestEntity("Test");
    public RepositoryTest()
    {
        _repository = new Repository<TestEntity>(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    [TestMethod]
    public void Add_WhenElementIsProvided_AddsElementToRepository()
    {
        var result = _repository.Add(_testEntity);

        result.Should().Be(_testEntity);
    }

    #endregion

    #region Update

    [TestMethod]
    public void Update_WhenElementIsProvided_UpdatesElementInRepository()
    {
        _repository.Add(_testEntity);

        _testEntity.Name = "Test Modified";
        var result = _repository.Update(_testEntity);

        result.Should().Be(_testEntity);
    }

    #endregion

    #region Get

    [TestMethod]
    public void Get_WhenElementIsProvided_ReturnsElementFromRepository()
    {
        _repository.Add(_testEntity);

        var result = _repository.Get(e => e.Id == _testEntity.Id);

        result.Should().Be(_testEntity);
    }

    #endregion

    #region GetAll

    [TestMethod]
    public void GetAll_WhenCalled_ReturnsAllElementsFromRepository()
    {
        var element2 = new TestEntity("Test 2");

        _repository.Add(_testEntity);
        _repository.Add(element2);

        var result = _repository.GetAll(_ => true);

        result.Should().Contain(new[] { _testEntity, element2 });
    }

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenElementIsProvided_ReturnsTrueIfElementExists()
    {
        _repository.Add(_testEntity);

        var result = _repository.Exists(e => e.Id == _testEntity.Id);

        result.Should().BeTrue();
    }

    #endregion

    #region Delete

    [TestMethod]
    public void Delete_WhenElementIsProvided_DeletesElementFromRepository()
    {
        _repository.Add(_testEntity);

        _repository.Delete(_testEntity);

        var result = _repository.Get(e => e.Id == _testEntity.Id);

        result.Should().BeNull();
    }

    #endregion
}

internal sealed class TestDbContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<TestEntity> EntitiesTest { get; set; }
}


internal sealed record TestEntity()
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public TestEntity(string name)
        : this()
    {
        Name = name;
    }
}
