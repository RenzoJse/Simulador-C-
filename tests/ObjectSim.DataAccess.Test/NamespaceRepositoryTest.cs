

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;
using ObjectSim.Domain;

namespace ObjectSim.DataAccess.Test;
[TestClass]
public class NamespaceRepositoryTest
{
    private SqliteConnection _connection = null!;
    private DataContext _context = null!;
    private NamespaceRepository _repository = null!;
    [TestInitialize]
    public void Setup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DataContext(options);
        _context.Database.EnsureCreated();

        _repository = new NamespaceRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
        _connection.Dispose();
    }
    [TestMethod]
    public void Add_ShouldPersistNamespace()
    {
        var ns = new Namespace { Name = "TestNamespace" };

        var result = _repository.Add(ns);

        var fromDb = _context.Namespaces.Find(result.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("TestNamespace", fromDb!.Name);
    }
    [TestMethod]
    public void GetAll_ShouldReturnAllNamespaces()
    {
        _context.Namespaces.AddRange(
            new Namespace { Name = "One" },
            new Namespace { Name = "Two" });
        _context.SaveChanges();

        var result = _repository.GetAll();

        Assert.AreEqual(2, result.Count);
    }
    [TestMethod]
    public void GetByIdWithChildren_ShouldReturnNamespaceWithNestedChildren()
    {
        var root = new Namespace { Name = "Root" };
        var child1 = new Namespace { Name = "Child1", ParentId = root.Id };
        var child2 = new Namespace { Name = "Child2", ParentId = child1.Id };

        root.Children.Add(child1);
        child1.Children.Add(child2);

        _context.Namespaces.AddRange(root, child1, child2);
        _context.SaveChanges();

        var result = _repository.GetByIdWithChildren(root.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Root", result!.Name);
        Assert.AreEqual(1, result.Children.Count);
        Assert.AreEqual("Child1", result.Children[0].Name);
        Assert.AreEqual(1, result.Children[0].Children.Count);
        Assert.AreEqual("Child2", result.Children[0].Children[0].Name);
    }

    [TestMethod]
    public void GetByIdWithChildren_ShouldBuildNestedStructureCorrectly_FromDatabase()
    {
        var rootId = Guid.NewGuid();
        var child1Id = Guid.NewGuid();
        var child2Id = Guid.NewGuid();

        var root = new Namespace { Id = rootId, Name = "Root" };
        var child1 = new Namespace { Id = child1Id, Name = "Child1", ParentId = rootId };
        var child2 = new Namespace { Id = child2Id, Name = "Child2", ParentId = child1Id };

        _context.Namespaces.AddRange(root, child1, child2);
        _context.SaveChanges();

        var result = _repository.GetByIdWithChildren(rootId);

        Assert.AreEqual("Root", result!.Name);
        Assert.AreEqual(1, result.Children.Count);
        Assert.AreEqual("Child1", result.Children[0].Name);
        Assert.AreEqual(1, result.Children[0].Children.Count);
    }
    [TestMethod]
    public void GetByIdWithChildren_WithNoChildren_ReturnsNodeOnly()
    {
        var root = new Namespace { Name = "Solo" };
        _context.Namespaces.Add(root);
        _context.SaveChanges();

        var result = _repository.GetByIdWithChildren(root.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Solo", result!.Name);
        Assert.AreEqual(0, result.Children.Count);
    }

    [TestMethod]
    public void GetByIdWithChildren_InvalidId_ReturnsNull()
    {
        var result = _repository.GetByIdWithChildren(Guid.NewGuid());

        Assert.IsNull(result);
    }
}
