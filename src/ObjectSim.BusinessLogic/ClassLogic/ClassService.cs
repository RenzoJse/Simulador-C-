using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassService(List<IBuilderStrategy> strategies, IRepository<Class> classRepository, IAttributeService attributeService) : IClassService
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

        CanAddMethod(classObj, method); //Tengo que ver lo de si abstract haga la clase abstract

        classObj.Methods!.Add(method);
    }

    private static void ValidateMethodUniqueness(Class classObj, Method method)
    {
        if (classObj.Methods!.Any(classMethod => classMethod.Name == method.Name &&
                                                 classMethod.Type == method.Type &&
                                                 method.IsOverride == false &&
                                                 AreParametersEqual(classMethod.Parameters, method.Parameters)))
        {
            throw new ArgumentException("Method already exists in class.");
        }
    }

    private static void ValidateInterfaceMethodConstraints(Method method)
    {
        if (method.IsSealed)
        {
            throw new ArgumentException("Method cannot be sealed in an interface.");
        }
        if (method.IsOverride)
        {
            throw new ArgumentException("Method cannot be override in an interface.");
        }
        if (method.Accessibility == Method.MethodAccessibility.Private)
        {
            throw new ArgumentException("Method cannot be private in an interface.");
        }
        if (method.LocalVariables.Count > 0)
        {
            throw new ArgumentException("Method cannot have local variables in an interface.");
        }
        if (method.MethodsInvoke.Count > 0)
        {
            throw new ArgumentException("Method cannot invoke other methods in an interface.");
        }
        if (!method.Abstract)
        {
            method.Abstract = true;
        }
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

    public bool CanAddMethod(Class classObj, Method method)
    {
        ValidateMethodUniqueness(classObj, method);

        if(classObj.IsInterface == true)
        {
            ValidateInterfaceMethodConstraints(method);
        }

        return true;
    }

    public void AddAttribute(Guid? classId, Guid? idAttribute)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(idAttribute);

        var classObj = GetById(classId);
        var attribute = attributeService.GetById((Guid)idAttribute);

        if (CanAddAttribute(classObj, attribute))
        {
            classObj.Attributes!.Add(attribute);
        }
    }

    public bool CanAddAttribute(Class classObj, Attribute attribute)
    {
        if(classObj.IsInterface == true)
        {
            throw new ArgumentException("Cannot add attribute to an interface.");
        }

        if (classObj.Attributes!.Any(classAttribute => classAttribute.Name == attribute.Name))
        {
            throw new ArgumentException("Attribute name already exists in class.");
        }

        return true;
    }

    public void DeleteClass(Guid? classId)
    {
        var classObj = GetById(classId);

        var hasDependentClasses = classRepository.GetAll(c => c.Parent != null)
            .Any(c => c.Parent != null && c.Parent.Id == classId);

        if (hasDependentClasses)
        {
            throw new ArgumentException("Cannot delete class that is implemented by another class.");
        }

        classRepository.Delete(classObj);
    }

    public void RemoveMethod(Guid? classId, Guid? methodId)
    {
        throw new NotImplementedException();
    }

    public void DeleteAttribute(Guid? classId, Guid? attributeId)
    {
        throw new NotImplementedException();
    }

}
