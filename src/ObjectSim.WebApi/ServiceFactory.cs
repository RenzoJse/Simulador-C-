using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ObjectSim.BusinessLogic;
using ObjectSim.ClassLogic.Strategy;
using ObjectSim.DataAccess;
using ObjectSim.DataAccess.Interface;
using ObjectSim.DataAccess.Repositories;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.WebApi;

[ExcludeFromCodeCoverage]
public static class ServiceFactory
{
    public static void AddContext(IServiceCollection services, string connectionString)
    {
        if(string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Connection string is required.");
        }

        services.AddDbContext<DbContext, DataContext>(options =>
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());
    }

    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IMethodService, MethodService>();
        services.AddScoped<IMethodServiceCreate>(sp => (IMethodServiceCreate)sp.GetRequiredService<IMethodService>());
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<IDataTypeService, DataTypeService>();

        services.AddScoped<IBuilderStrategy, ClassBuilderStrategy>();
        services.AddScoped<IBuilderStrategy, InterfaceBuilderStrategy>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IClassServiceBuilder>(sp => (IClassServiceBuilder)sp.GetRequiredService<IClassService>());
    }

    public static void AddDataAccess(IServiceCollection services)
    {
        services.AddScoped<IRepository<Class>, ClassRepository>();
        services.AddScoped<IRepository<Method>, Repository<Method>>();
        services.AddScoped<IRepository<Attribute>, Repository<Attribute>>();
        services.AddScoped<IRepository<DataType>, Repository<DataType>>();
    }
}
