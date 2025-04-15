using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;

namespace ObjectSim.DataAccess.Test;

[TestClass]
public class RepositoryTest
{
    private readonly DbContext _context = ContextConstructorDb.CreateMemoryContext();
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

    #region Update

    [TestMethod]
    public void Update_WhenElementIsProvided_UpdatesElementInRepository()
    {
        var element = new TestEntity("Test");

        _repository.Add(element);

        element.Name = "Test Modified";
        var result = _repository.Update(element);

        result.Should().Be(element);
    }

    #endregion

    #region Get

    [TestMethod]
    public void Get_WhenElementIsProvided_ReturnsElementFromRepository()
    {
        var element = new TestEntity("Test");

        _repository.Add(element);

        var result = _repository.Get(element.Id);

        result.Should().Be(element);
    }

    #endregion

    #region GetAll

    [TestMethod]
    public void GetAll_WhenCalled_ReturnsAllElementsFromRepository()
    {
        var element1 = new TestEntity("Test 1");
        var element2 = new TestEntity("Test 2");

        _repository.Add(element1);
        _repository.Add(element2);

        var result = _repository.GetAll();

        result.Should().Contain(new[] { element1, element2 });
    }

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenElementIsProvided_ReturnsTrueIfElementExists()
    {
        var element = new TestEntity("Test");

        _repository.Add(element);

        var result = _repository.Exists(e => e.Id == element.Id);

        result.Should().BeTrue();
    }

    #endregion

    #region Delete

    [TestMethod]
    public void Delete_WhenElementIsProvided_DeletesElementFromRepository()
    {
        var element = new TestEntity("Test");

        _repository.Add(element);

        _repository.Delete(element);

        var result = _repository.Get(element.Id);

        result.Should().BeNull();
    }

    #endregion
}

internal sealed class TestDbContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<TestEntity> EntitiesTest { get; set; }
}


internal sealed record class TestEntity()
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public TestEntity(string name)
        : this()
    {
        Name = name;
    }
}
