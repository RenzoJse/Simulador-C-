using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.Strategy;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic;

public class ClassService(IEnumerable<IBuilderStrategy> strategies, IRepository<Class> classRepository) : IClassService
{

    #region CreateClass

    public Class CreateClass(CreateClassArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var builder = GetBuilder(args);
        builder.SetName(args.Name!);
        builder.SetParent(args.Parent == null ? null : GetById(args.Parent));
        builder.SetAbstraction(args.IsAbstract);
        builder.SetInterface(args.IsInterface);
        builder.SetSealed(args.IsSealed);
        builder.SetAttributes(args.Attributes);
        builder.SetMethods(args.Methods);
        var classObj = builder.GetResult();
        AddClassToRepository(classObj);
        return classObj;
    }

    private void AddClassToRepository(Class classObj)
    {
        classRepository.Add(classObj);
    }

    private Builder GetBuilder(CreateClassArgs args)
    {
        var strategy = strategies.FirstOrDefault(x => x.WhichIsMyBuilder(args));

        return strategy!.CreateBuilder();
    }

    #endregion

    #region GetById

    public Class GetById(Guid? classId)
    {
        if(classId == null)
        {
            throw new ArgumentNullException(nameof(classId));
        }
        return classRepository.Get(c => c.Id == classId) ?? throw new ArgumentException("Class not found.");
    }

    #endregion

    #region DeleteClass

    public void DeleteClass(Guid? classId)
    {
        var classObj = GetById(classId);

        var hasDependentClasses = classRepository.GetAll(c => c.Parent != null)
            .Any(c => c.Parent != null && c.Parent.Id == classId);

        if(hasDependentClasses)
        {
            throw new ArgumentException("Cannot delete class that is implemented by another class.");
        }

        classRepository.Delete(classObj);
    }

    #endregion

    #region RemoveMethod

     public void RemoveMethod(Guid? classId, Guid? methodId)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(methodId);

        var classObj = GetById(classId);
        ValidateIfClassHasMethods(classObj);

        var method = GetMethodFromClass(classObj, methodId);
        ValidateParentClassConstraints(classObj, method);
        ValidateMethodDependencies(classObj.Methods!, method);

        classObj.Methods!.Remove(method);
    }

    private static void ValidateIfClassHasMethods(Class classObj)
    {
        if(classObj.Methods == null || classObj.Methods.Count == 0)
        {
            throw new ArgumentException("Class has no methods.");
        }
    }

    private static Method GetMethodFromClass(Class classObj, Guid? methodId)
    {
        var method = classObj.Methods!.FirstOrDefault(m => m.Id == methodId);
        if(method == null)
        {
            throw new ArgumentException("Method not found in class.");
        }
        return method;
    }

    private static void ValidateParentClassConstraints(Class classObj, Method method)
    {
        if(classObj.Parent == null)
        {
            return;
        }

        var parentClass = classObj.Parent;

        if((bool)parentClass.IsInterface!)
        {
            ValidateInterfaceMethod(parentClass, method);
        }

        if((bool)parentClass.IsAbstract! && method.IsOverride)
        {
            throw new ArgumentException("Cannot remove method that is overriding abstract parent method you implement.");
        }
    }

    private static void ValidateInterfaceMethod(Class parentClass, Method method)
    {
        try
        {
            parentClass.ValidateMethodUniqueness(method);
        }
        catch(Exception)
        {
            throw new ArgumentException("Cannot remove method that is in an interface you implement.");
        }
    }

    private static void ValidateMethodDependencies(List<Method> methodList, Method method)
    {
        foreach(var methodInClass in methodList)
        {
            if(methodInClass.MethodsInvoke != null && methodInClass.MethodsInvoke.Contains(method))
            {
                throw new ArgumentException("Cannot remove method that is invoked by another method.");
            }
        }
    }

    #endregion

    #region RemoveAttribute

    public void RemoveAttribute(Guid classId, Guid attributeId)
    {
        var classObj = GetById(classId);
        ValidateIfClassHasAttributes(classObj);

        var attribute = GetAttributeFromClass(classObj, attributeId);
        ValidateAttributeNotUsedInMethods(classObj, attribute);

        classObj.Attributes!.Remove(attribute);
        classRepository.Update(classObj);
    }

    private static void ValidateIfClassHasAttributes(Class classObj)
    {
        if(classObj.Attributes == null || classObj.Attributes.Count == 0)
        {
            throw new ArgumentException("The class have no attributes.");
        }
    }

    private static Attribute GetAttributeFromClass(Class classObj, Guid? attributeId)
    {
        var attribute = classObj.Attributes!.FirstOrDefault(a => a.Id == attributeId);
        if(attribute == null)
        {
            throw new ArgumentException("That attribute does not exist in the class.");
        }
        return attribute;
    }

    private static void ValidateAttributeNotUsedInMethods(Class classObj, Attribute attribute)
    {
        if(classObj.Methods == null || classObj.Methods.Count == 0)
        {
            return;
        }

        foreach(var method in classObj.Methods)
        {
            if(method.LocalVariables != null && method.LocalVariables.Any(lv => lv.Name == attribute.Name))
            {
                throw new ArgumentException("Attribute is being used in method.");
            }
        }
    }

    #endregion

}
