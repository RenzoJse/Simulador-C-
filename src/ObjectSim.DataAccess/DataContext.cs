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
    public DbSet<LocalVariable> LocalVariables { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
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

        modelBuilder.Entity<Method>(m =>
        {
            m.HasKey(m => m.Id);

            m.HasMany(m => m.Parameters)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            m.HasMany(m => m.LocalVariables)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            m.HasMany(m => m.MethodsInvoke)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attribute>(a =>
        {
            a.HasKey(a => a.Id);
            a.Ignore(a => a.DataType);
        });

        modelBuilder.Entity<Parameter>(p =>
        {
            p.HasKey(p => p.Id);
        });

        modelBuilder.Entity<LocalVariable>(lv =>
        {
            lv.HasKey(lv => lv.Id);
        });

        modelBuilder.Entity<ValueType>(vt =>
        {
            vt.HasKey(vt => vt.Name);
            vt.Property(vt => vt.Name).IsRequired();
        });

        modelBuilder.Entity<ReferenceType>(rt =>
        {
            rt.HasKey(rt => rt.Name);
            rt.Property(rt => rt.Name).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
