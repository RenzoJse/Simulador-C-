using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassService(List<IBuilderStrategy> strategies, IRepository<Class> classRepository) : IClassService
{
    public Class CreateClass(CreateClassArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var builder = GetBuilder(args);
        builder.SetParent(args.Parent);
        builder.SetName(args.Name!);
        builder.SetAbstraction(args.IsAbstract);
        builder.SetInterface(args.IsInterface);
        builder.SetSealed(args.IsSealed);
        builder.SetAttributes(args.Attributes);
        builder.SetMethods(args.Methods);
        return builder.GetResult()!;
    }

    private Builder GetBuilder(CreateClassArgs args)
    {
        var strategy = strategies.FirstOrDefault(x => x.WhichIsMyBuilder(args));

        return strategy!.CreateBuilder();
    }

    public Class GetById(Guid? classId)
    {
        if (classId == null)
        {
            throw new ArgumentNullException(nameof(classId));
        }
        return classRepository.Get(c => c.Id == classId) ?? throw new ArgumentException("Class not found.");
    }

    public void AddMethod(Guid? classId, Method? method)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(method);

        var classObj = GetById(classId);

        if(classObj.IsInterface == true)
        {

        }

        if(classObj is { IsAbstract: false, IsInterface: false })
        {

        }

        foreach (var classMethod in classObj.Methods!)
        {
            if (classMethod.Name == method.Name &&
                classMethod.Type == method.Type &&
                AreParametersEqual(classMethod.Parameters, method.Parameters))
            {
                //Falta lo de Overriding
                throw new ArgumentException("Method already exists in class.");
            }
        }
        classObj.Methods!.Add(method);
    }

    private static bool AreParametersEqual(List<Parameter> parameters1, List<Parameter> parameters2)
    {
        if(parameters1.Count != parameters2.Count)
        {
            return false;
        }

        for (var i = 0; i < parameters1.Count; i++)
        {
            var p1 = parameters1[i];
            var p2 = parameters2[i];

            if (p1.Name != p2.Name || p1.Type != p2.Type)
            {
                return false;
            }
        }

        return true;
    }

    public void AddAttribute(Guid? classId, Attribute attribute)
    {
        throw new NotImplementedException();
    }

    public bool CanAddAttribute(Class classObj, Attribute attribute)
    {
        throw new NotImplementedException();
    }

    public bool CanAddMethod(Class classObj, Method method)
    {
        throw new NotImplementedException();
    }
}
