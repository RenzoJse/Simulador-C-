
using ObjectSim.Domain;

namespace ObjectSim.Examples;
public static class VisitorExampleFactory
{
    public static List<Class> CreateVisitorExample()
    {
        var circleId = Guid.NewGuid();
        var rectangleId = Guid.NewGuid();
        var exporterId = Guid.NewGuid();
        var visitCircleId = Guid.NewGuid();
        var visitRectangleId = Guid.NewGuid();
        var acceptCircleId = Guid.NewGuid();
        var acceptRectangleId = Guid.NewGuid();

        var circle = new Class
        {
            Id = circleId,
            Name = "Circle",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = []
        };

        var rectangle = new Class
        {
            Id = rectangleId,
            Name = "Rectangle",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = []
        };

        var exporter = new Class
        {
            Id = exporterId,
            Name = "SvgExporter",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = []
        };

        var visitCircle = new Method
        {
            Id = visitCircleId,
            Name = "VisitCircle",
            ClassId = exporterId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(circleId, "circle")]
        };

        var visitRectangle = new Method
        {
            Id = visitRectangleId,
            Name = "VisitRectangle",
            ClassId = exporterId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(rectangleId, "rectangle")]
        };

        exporter.Methods!.Add(visitCircle);
        exporter.Methods!.Add(visitRectangle);

        var acceptCircle = new Method
        {
            Id = acceptCircleId,
            Name = "Accept",
            ClassId = circleId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(exporterId, "visitor")],
            MethodsInvoke = [
                new InvokeMethod(acceptCircleId, visitCircleId, "visitor")
            ]
        };

        var acceptRectangle = new Method
        {
            Id = acceptRectangleId,
            Name = "Accept",
            ClassId = rectangleId,
            TypeId = Guid.Empty,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [new Variable(exporterId, "visitor")],
            MethodsInvoke = [
                new InvokeMethod(acceptRectangleId, visitRectangleId, "visitor")
            ]
        };

        circle.Methods!.Add(acceptCircle);
        rectangle.Methods!.Add(acceptRectangle);

        return [circle, rectangle, exporter];
    }
}
