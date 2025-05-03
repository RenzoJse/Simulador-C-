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
                .WithOne(m => m.Class)
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

            m.HasOne(m => m.Type)
                .WithMany()
                .HasForeignKey("TypeId");

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

        base.OnModelCreating(modelBuilder);
        ModelSeedData();
    }
    private void ModelSeedData()
    {
        //insertar en la bd todos los metodos predefinidos por .NET para cada tipo de dato.
        //Tienen que tener guid fijo.
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
