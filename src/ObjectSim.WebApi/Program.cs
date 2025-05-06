using System.Diagnostics.CodeAnalysis;
using ObjectSim.WebApi;
using ObjectSim.WebApi.Filter;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        b => b.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var services = builder.Services;
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("ObjectSim");

ServiceFactory.AddContext(services, connectionString!);
ServiceFactory.AddServices(services);
ServiceFactory.AddDataAccess(services);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public abstract partial class Program
{
}
