using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.DataAccess;

[ExcludeFromCodeCoverage]
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Class> Classes { get; set; }
    public DbSet<Method> Methods { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<DataType> DataTypes { get; set; }
    public DbSet<ValueType> ValueTypes { get; set; }
    public DbSet<ReferenceType> ReferenceTypes { get; set; }
    public DbSet<InvokeMethod> InvokeMethod { get; set; }
    public DbSet<Variable> Variables { get; set; }
    public DbSet<Namespace> Namespaces { get; set; }
    public DbSet<Key> Keys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(
                    "Server=.\\SQLEXPRESS;Database=ObjectSim; Integrated Security=True; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate=True")
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Key>(k =>
        {
            k.HasKey(k => k.AccessKey);
        });

        modelBuilder.Entity<Class>(c =>
        {
            c.HasKey(c => c.Id);

            c.HasMany(c => c.Methods)
                .WithOne()
                .HasForeignKey(m => m.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            c.HasMany(c => c.Attributes)
                .WithOne()
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            c.HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey("ParentId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReferenceType>(rt =>
        {
            rt.Property(r => r.Type).IsRequired();
        });

        modelBuilder.Entity<ValueType>(vt =>
        {
            vt.Property(v => v.Type).IsRequired();
        });

        modelBuilder.Entity<Method>(m =>
        {
            m.HasKey(m => m.Id);

            m.HasOne<DataType>()
                .WithMany()
                .HasForeignKey(m => m.TypeId)
                .OnDelete(DeleteBehavior.Cascade);

            m.HasMany(m => m.MethodsInvoke)
                .WithOne()
                .HasForeignKey(m => m.MethodId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Attribute>(a =>
        {
            a.HasKey(a => a.Id);
            a.HasOne(a => a.DataType)
                .WithMany()
                .HasForeignKey(a => a.DataTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DataType>(dt =>
        {
            dt.HasKey(d => d.Id);
            dt.HasDiscriminator<string>("Discriminator")
                .HasValue<DataType>("DataType")
                .HasValue<ValueType>("ValueType")
                .HasValue<ReferenceType>("ReferenceType");
        });

        modelBuilder.Entity<ValueType>(vt =>
        {
            vt.Property(vt => vt.Type).IsRequired();
        });

        modelBuilder.Entity<ReferenceType>(rt =>
        {
            rt.Property(rt => rt.Type).IsRequired();
        });

        modelBuilder.Entity<InvokeMethod>(im =>
        {
            im.HasKey(im => new { im.MethodId, im.InvokeMethodId });
        });

        var guidListToStringConverter = new ValueConverter<List<Guid>, string>(
            v => string.Join(";", v.Select(g => g.ToString())),
            v => v.Split(';', StringSplitOptions.RemoveEmptyEntries)
                  .Select(Guid.Parse).ToList()
        );

        modelBuilder.Entity<Namespace>(n =>
        {
            n.HasMany(ns => ns.Children)
                .WithOne()
                .HasForeignKey(ns => ns.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            n.Property(ns => ns.ClassIdsSerialized)
                .HasColumnName("ClassIdsSerialized");
            n.Ignore(ns => ns.Classes);
        });
        modelBuilder.Entity<Variable>(v =>
        {
            v.HasKey(v => v.VariableId);

            v.HasOne(v => v.Method)
                .WithMany()
                .HasForeignKey(v => v.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
        ModelSeedData(modelBuilder);
    }

    private void ModelSeedData(ModelBuilder modelBuilder)
    {
        var voidTypeId = Guid.Parse("00000000-0000-0000-0000-000000000005");

        modelBuilder.Entity<ValueType>().HasData(new ValueType
        {
            Id = voidTypeId,
            Type = "void"
        });

        var objectClassId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var objectClass = new Class
        {
            Id = objectClassId,
            Name = "Object",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false
        };

        modelBuilder.Entity<Class>().HasData(objectClass);

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "String",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Name = "Int32",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
            Name = "Boolean",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
            Name = "Char",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
            Name = "Decimal",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
            Name = "Byte",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
            Name = "float",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        modelBuilder.Entity<Class>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
            Name = "Double",
            ParentId = objectClass.Id,
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false
        });

        var valueTypes = new List<ValueType>
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Type = "int"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Type = "bool"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                Type = "char"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                Type = "decimal"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                Type = "byte"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                Type = "float"
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                Type = "double"
            }
        };

        modelBuilder.Entity<ValueType>().HasData(valueTypes);

        modelBuilder.Entity<Method>().HasData(
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
                Name = "Equals",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
                Name = "Equals (Object, Object)",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000103"),
                Name = "Finalize",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Protected,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000104"),
                Name = "GetHashCode",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000105"),
                Name = "GetType",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000106"),
                Name = "MemberwiseClone",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Protected,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000107"),
                Name = "ReferenceEquals",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            },
            new Method
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000108"),
                Name = "ToString",
                ClassId = objectClassId,
                Accessibility = Method.MethodAccessibility.Public,
                Abstract = false,
                IsSealed = false,
                TypeId = voidTypeId,
                IsOverride = false
            }

        );
        modelBuilder.Entity<Key>().HasData(
            new Key { AccessKey = Guid.Parse("9C0FF0B1-4ABD-45C6-8A4A-831748FB7A20") },
            new Key { AccessKey = Guid.Parse("515DD649-30A0-4D57-9302-62A8DB8179BD") }
        );
        var defaultAttributeId = Guid.Parse("00000000-0000-0000-0000-000000000201");
        modelBuilder.Entity<Attribute>().HasData(new Attribute
        {
            Id = defaultAttributeId,
            Name = "default",
            ClassId = objectClassId,
            DataTypeId = voidTypeId,
            Visibility = Attribute.AttributeVisibility.Public,
            IsStatic = false
        });

    }
}
