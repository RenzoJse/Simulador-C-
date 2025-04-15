using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.DataAccess;

[ExcludeFromCodeCoverage]
public class DataContext : DbContext
{
    public DbSet<Class> Classes { get; set; }
    public DbSet<Method> Methods { get; set; }
    public DbSet<Attribute> Attributes { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
}
