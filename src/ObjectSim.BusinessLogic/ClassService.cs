using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.Strategy;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic;

public class ClassService(IDataTypeService dataTypeService, IEnumerable<IBuilderStrategy> builderStrategies, IRepository<Class> classRepository) : IClassService
{

    #region CreateClass

    public Class CreateClass(CreateClassArgs args)
    {
        CheckForDuplicateName(args.Name!);
        var builder = SelectBuilderForArgs(args);
        ConfigureBuilderWithArgs(builder, args);

        var newClass = builder.GetResult();
        SaveClassToRepository(newClass);
        CreateDataType(newClass);

        return newClass;
    }

    private void CheckForDuplicateName(string name)
    {
        if (classRepository.Get(c => c.Name == name) != null)
        {
            throw new ArgumentException("A class with that name already exists.");
        }
    }

    private void CreateDataType(Class newClass)
    {
        var dataTypeArgs = new CreateDataTypeArgs(newClass.Id, newClass.Name!);
        dataTypeService.CreateDataType(dataTypeArgs);
    }

    private Builder SelectBuilderForArgs(CreateClassArgs args)
    {
        var strategy = builderStrategies.FirstOrDefault(s => s.WhichIsMyBuilder(args));
        if(strategy == null)
        {
            throw new ArgumentException("No builder strategy found for the given arguments.");
        }

        return strategy.CreateBuilder();
    }

    private void ConfigureBuilderWithArgs(Builder builder, CreateClassArgs args)
    {
        builder.SetName(args.Name!);
        builder.SetParent(args.Parent == null ? null : GetById(args.Parent));
        builder.SetAbstraction(args.IsAbstract);
        builder.SetInterface(args.IsInterface);
        builder.SetSealed(args.IsSealed);
        builder.SetAttributes(args.Attributes);
        builder.SetMethods(args.Methods);
    }

    private void SaveClassToRepository(Class classObj)
    {
        classRepository.Add(classObj);
    }

    #endregion

    #region GetById

    public Class GetById(Guid? classId)
    {
        ValidateNullClassId(classId);
        return GetClassById((Guid)classId!) ?? throw new ArgumentException("Class not found.");
    }

    private static void ValidateNullClassId(Guid? classId)
    {
        if(classId == null)
        {
            throw new ArgumentNullException(nameof(classId));
        }
    }

    private Class? GetClassById(Guid classId)
    {
        return classRepository.Get(c => c.Id == classId);
    }

    #endregion

    #region GetAll

    public List<Class> GetAll()
    {
        var classes = classRepository.GetAll(c => c.Id != Guid.Empty)?.ToList();
        if(classes == null || classes.Count == 0)
        {
            throw new Exception("No classes found.");
        }

        return classes;
    }

    #endregion

    #region SystemMethod

    public Guid GetIdByName(string name)
    {
        var foundClass = classRepository.Get(c => c.Id != Guid.Empty && c.Name == name);

        if (foundClass == null)
        {
            throw new ArgumentException("Class not found");
        }

        return foundClass.Id;
    }

    #endregion

    #region DeleteClass

    public void DeleteClass(Guid? classId)
    {
        var classToDelete = GetById(classId);
        EnsureClassIsNotParent(classId);
        classRepository.Delete(classToDelete);
    }

    private void EnsureClassIsNotParent(Guid? classId)
    {
        var hasDependentClasses = classRepository.GetAll(c => c.Parent != null)
            .Any(c => c.Parent != null && c.Parent.Id == classId);

        if(hasDependentClasses)
        {
            throw new ArgumentException("Cannot delete class that is implemented by another class.");
        }
    }

    #endregion

    #region RemoveMethod

    public void RemoveMethod(Guid? classId, Guid? methodId)
    {
        ValidateRemoveMethodArgs(classId, methodId);

        var classObj = GetById(classId);
        EnsureClassHasMethods(classObj);

        var method = FindMethodInClass(classObj, methodId);
        EnsureMethodCanBeRemoved(classObj, method);

        classObj.Methods!.Remove(method);
    }

    private static void ValidateRemoveMethodArgs(Guid? classId, Guid? methodId)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(methodId);
    }

    private static void EnsureClassHasMethods(Class classObj)
    {
        if(classObj.Methods == null || classObj.Methods.Count == 0)
        {
            throw new ArgumentException("Class has no methods.");
        }
    }

    private static Method FindMethodInClass(Class classObj, Guid? methodId)
    {
        var method = classObj.Methods!.FirstOrDefault(m => m.Id == methodId);
        if(method == null)
        {
            throw new ArgumentException("Method not found in class.");
        }

        return method;
    }

    private static void EnsureMethodCanBeRemoved(Class classObj, Method method)
    {
        ValidateParentClassConstraints(classObj, method);
        ValidateMethodIsNotInvokedByOthers(classObj.Methods!, method);
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
            EnsureMethodNotInInterface(parentClass, method);
        }

        if((bool)parentClass.IsAbstract! && method.IsOverride)
        {
            throw new ArgumentException("Cannot remove method that is overriding abstract parent method you implement.");
        }
    }

    private static void EnsureMethodNotInInterface(Class parentClass, Method method)
    {
        try
        {
            parentClass.ValidateMethodUniqueness(method);
        }
        catch
        {
            throw new ArgumentException("Cannot remove method that is in an interface you implement.");
        }
    }

    private static void ValidateMethodIsNotInvokedByOthers(List<Method> methods, Method method)
    {
        var isInvoked = methods
            .SelectMany(m => m.MethodsInvoke)
            .Any(invocation => invocation.InvokeMethodId == method.Id);

        if(isInvoked)
        {
            throw new ArgumentException("Cannot remove method that is invoked by another method.");
        }
    }

    #endregion

    #region RemoveAttribute

    public void RemoveAttribute(Guid classId, Guid attributeId)
    {
        var classObj = GetById(classId);
        EnsureClassHasAttributes(classObj);

        var attribute = FindAttributeInClass(classObj, attributeId);
        EnsureAttributeNotUsedInMethods(classObj, attribute);

        classObj.Attributes!.Remove(attribute);
        classRepository.Update(classObj);
    }

    private static void EnsureClassHasAttributes(Class classObj)
    {
        if(classObj.Attributes == null || classObj.Attributes.Count == 0)
        {
            throw new ArgumentException("The class has no attributes.");
        }
    }

    private static Attribute FindAttributeInClass(Class classObj, Guid? attributeId)
    {
        var attribute = classObj.Attributes!.FirstOrDefault(a => a.Id == attributeId);
        if(attribute == null)
        {
            throw new ArgumentException("That attribute does not exist in the class.");
        }

        return attribute;
    }

    private static void EnsureAttributeNotUsedInMethods(Class classObj, Attribute attribute)
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

    #region UpdateClass
    public void UpdateClass(Guid classId, string newName)
    {
        var existingClass = classRepository.Get(c => c.Id == classId)
            ?? throw new ArgumentException("Class not found");

        if(string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        existingClass.Name = newName;
        classRepository.Update(existingClass);
    }
    #endregion

}
