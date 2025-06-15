
using ObjectSim.Domain;

using ObjectSim.DataAccess.Interface;

namespace ObjectSim.Examples;
public  class VisitorExampleFactory(IRepository<Class> classRepository, IRepository<Method> methodRepository):IExampleService
{
    public void CreateExample()
    {
        var circleId = Guid.NewGuid();
        var rectangleId = Guid.NewGuid();
        var exporterId = Guid.NewGuid();
        var visitCircleId = Guid.NewGuid();
        var visitRectangleId = Guid.NewGuid();
        var acceptCircleId = Guid.NewGuid();
        var acceptRectangleId = Guid.NewGuid();
        var shapeClassId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var voidGuid = Guid.Parse("00000000-0000-0000-0000-000000000005");



        var visitCircle = new Method
        {
            Id = visitCircleId,
            Name = "VisitCircle",
            Abstract = true,
            ClassId = exporterId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(circleId, "circle")]
        };

        var visitRectangle = new Method
        {
            Id = visitRectangleId,
            Name = "VisitRectangle",
            Abstract = true,
            ClassId = exporterId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(rectangleId, "rectangle")]
        };


        var visitor=new Class
         {
             Id = visitorId,
             Name = "Visitor",
             IsAbstract = true,
             IsInterface = true,
             IsSealed = false,
             Attributes = [],
             Methods = [visitCircle,visitRectangle]
         };


        var accept=new Method
        {
            Id = Guid.NewGuid(),
            Name = "Accept",
            Abstract = true,
            ClassId = shapeClassId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(visitor.Id, "visitor")],
            MethodsInvoke = [
            ]
        };

        var shape = new Class
        {
            Id = shapeClassId,
            Name = "Shape",
            IsAbstract = true,
            IsInterface = true,
            IsSealed = false,
            Attributes = [],
            Methods = [accept]
        };


        var circle = new Class
        {
            Id = circleId,
            Parent = shape,
            Name = "Circle",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = []
        };
        var acceptCircle = new Method
        {
            Id = acceptCircleId,
            Name = "Accept",
            IsOverride = true,
            Abstract = true,
            ClassId = circleId,
            TypeId = voidGuid,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(visitorId, "visitor")],
            MethodsInvoke = [new InvokeMethod(acceptCircleId, visitCircleId, "visitor")]
        };

        var rectangle = new Class
        {
            Id = rectangleId,
            Parent = shape,
            Name = "Rectangle",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = []
        };



        var acceptRectangle = new Method
        {
            Id = acceptRectangleId,
            Name = "Accept",
            IsOverride = true,
            Abstract = true,
            ClassId = rectangleId,
            TypeId = voidGuid,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(visitorId, "visitor")],
            MethodsInvoke = [
                new InvokeMethod(acceptRectangleId, visitRectangleId, "visitor")
            ]
        };

        var visitCircleOverride = new Method
        {
            Id = visitCircleId,
            Name = "VisitCircle",
            ClassId = exporterId,
            TypeId = voidGuid,
            Abstract = true,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(circleId, "circle")],
            IsOverride = true
        };
        var visitRectangleOverride = new Method
        {
            Id = visitRectangleId,
            Name = "VisitRectangle",
            ClassId = exporterId,
            Abstract=true,
            TypeId = voidGuid,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(rectangleId, "rectangle")],
            IsOverride = true
        };

        var sizeVisitor = new Class
        {
            Id = exporterId,
            Name = "SizeVisitor",
            Parent = visitor,
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = [visitCircleOverride, visitRectangleOverride]
        };

        classRepository.Add(circle);
        classRepository.Add(rectangle);
        classRepository.Add(shape);
        classRepository.Add(visitor);
        classRepository.Add(sizeVisitor);

        methodRepository.Add(visitCircle);
        methodRepository.Add(visitRectangle);
        methodRepository.Add(accept);
        methodRepository.Add(acceptCircle);
        methodRepository.Add(acceptRectangle);
        methodRepository.Add(visitCircleOverride);
        methodRepository.Add(visitRectangleOverride);

    }
}
