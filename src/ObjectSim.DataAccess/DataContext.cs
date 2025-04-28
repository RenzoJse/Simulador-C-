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
                .WithOne(m => m.Class)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
            c.HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey("ParentId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReferenceType>(rt =>
        {
            rt.HasKey(r => r.Name);
            rt.Property(r => r.Name).IsRequired();
        });

        modelBuilder.Entity<ValueType>(vt =>
        {
            vt.HasKey(v => v.Name);
            vt.Property(v => v.Name).IsRequired();
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

        base.OnModelCreating(modelBuilder);
    }
}
