using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
            rt.Property(r => r.Name).IsRequired();
        });

        modelBuilder.Entity<ValueType>(vt =>
        {
            vt.Property(v => v.Name).IsRequired();
        });

        modelBuilder.Entity<Method>(m =>
        {
            m.HasKey(m => m.Id);

            m.HasOne<DataType>()
                .WithMany()
                .HasForeignKey(m => m.TypeId)
                .OnDelete(DeleteBehavior.Cascade);

            m.HasMany(m => m.Parameters)
                .WithMany()
                .UsingEntity<DataTypeMethodParameters>(
                    j => j
                        .HasOne<DataType>()
                        .WithMany()
                        .HasForeignKey("IdDataType")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Method>()
                        .WithMany()
                        .HasForeignKey("IdMethod")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("IdDataType", "IdMethod");
                    });

            m.HasMany(m => m.LocalVariables)
                .WithMany()
                .UsingEntity<DataTypeMethodLocalVariables>(
                    j => j
                        .HasOne<DataType>()
                        .WithMany()
                        .HasForeignKey("IdDataType")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Method>()
                        .WithMany()
                        .HasForeignKey("IdMethod")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("IdDataType", "IdMethod");
                    });

            m.HasMany(m => m.MethodsInvoke)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attribute>(a =>
        {
            a.HasKey(a => a.Id);
            a.Ignore(a => a.DataType);
            a.HasOne(a => a.DataType)
                .WithMany()
                .HasForeignKey("DataTypeId")
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
            vt.Property(vt => vt.Name).IsRequired();
        });

        modelBuilder.Entity<ReferenceType>(rt =>
        {
            rt.Property(rt => rt.Name).IsRequired();
        });

        modelBuilder.Entity<InvokeMethod>(im =>
        {
            im.HasKey(im => new { im.MethodId, im.InvokeMethodId });

            im.HasOne<Method>()
                .WithMany()
                .HasForeignKey(im => im.MethodId)
                .OnDelete(DeleteBehavior.Restrict);

            im.HasOne<Method>()
                .WithMany(m => m.MethodsInvoke)
                .HasForeignKey(im => im.InvokeMethodId)
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
            Name = "void",
            Type = "void",
            MethodIds = []
        });
        var objectClassId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        modelBuilder.Entity<Class>().HasData(new Class
        {
            Id = objectClassId,
            Name = "Object",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false
        });
        var valueTypes = new List<ValueType>
{
    new ValueType
    {
        Id = Guid.Parse("249d6656-0276-556c-a992-bcf6bfea8578"),
        Name = "int",
        Type = "int",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("729965ef-64e3-5607-939f-8e19784ef0e9"),
        Name = "bool",
        Type = "bool",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("49e4ea3e-e6d6-4eb7-a7de-01cf4dc1cf7a"),
        Name = "char",
        Type = "char",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("1d9cd43c-e19b-4b24-ae0f-fb6cc43f1f27"),
        Name = "decimal",
        Type = "decimal",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("4e82822e-e6e1-44c1-9df9-7c43f7ecda5e"),
        Name = "byte",
        Type = "byte",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("75dfd62e-8d7c-48ee-9481-183ec3629936"),
        Name = "float",
        Type = "float",
        MethodIds = []
    },
    new ValueType
    {
        Id = Guid.Parse("bd8e7c9e-e8d0-42f2-9479-63284c5c3fa0"),
        Name = "double",
        Type = "double",
        MethodIds = []
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
    }

    [ExcludeFromCodeCoverage]
    public sealed record DataTypeMethodParameters
    {
        public Guid IdDataType { get; set; }
        public Guid IdMethod { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed record DataTypeMethodLocalVariables
    {
        public Guid IdDataType { get; set; }
        public Guid IdMethod { get; set; }
    }

}
