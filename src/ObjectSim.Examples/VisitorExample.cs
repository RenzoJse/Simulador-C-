using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.Examples;

public class VisitorExample(IClassService classService, IMethodService methodService) : IExampleService
{
    private static readonly Guid VoidGuid = Guid.Parse("00000000-0000-0000-0000-000000000005");
    private static readonly Guid ToStringGuid = Guid.Parse("00000000-0000-0000-0000-000000000108");
    private static readonly Guid Int = Guid.Parse("00000000-0000-0000-0000-000000000003");

    public void CreateExample()
    {
        var shapeInterface = CreateShapeInterface();
        var visitorInterface = CreateVisitorInterface();

        var circleShapeClass = CreateCircleShapeClass(shapeInterface.Id);
        var squareShapeClass = CreateSquareShapeClass(shapeInterface.Id);

        var visitCircleInterfaceMethod = CreateVisitCircleInterfaceMethod(visitorInterface.Id);
        var visitSquareInterfaceMethod = CreateVisitSquareInterfaceMethod(visitorInterface.Id, squareShapeClass.Id);

        var acceptMethodInterface = CreateAcceptMethodInterface(visitorInterface.Id, shapeInterface.Id);

        var visitorExportClass = CreateVisitorExportClass(visitorInterface.Id, circleShapeClass.Id, squareShapeClass.Id);
        var methodsList = visitorExportClass.Methods;
        foreach (var method in methodsList!)
        {
            AddInvokeIfMatch(method, "visitSquare", "square");
            AddInvokeIfMatch(method, "visitCircle", "circle");
        }

        var acceptMethodOverrideCircle = CreateAcceptMethodOverrideCircle(visitorInterface.Id, visitCircleInterfaceMethod.Id);
        var acceptMethodOverrideSquare = CreateAcceptMethodOverrideSquare(visitorInterface.Id, visitSquareInterfaceMethod.Id);
    }

    private void AddInvokeIfMatch(Method method, string expectedName, string paramName)
    {
        if (method.Name == expectedName)
        {
            var invokeMethodArgs = new CreateInvokeMethodArgs(ToStringGuid, paramName);
            methodService.AddInvokeMethod(method.Id, [invokeMethodArgs]);
        }
    }

    #region ExportVisitor

    private Class CreateVisitorExportClass(Guid visitorInterfaceId, Guid circleShapeId, Guid squareShapeId)
    {
        var visitCircleOverrideMethod = VisitCircleOverrideMethodArgs(circleShapeId);
        var visitSquareOverrideMethod = VisitSquareOverrideMethodArgs(squareShapeId);

        var visitorExportArgs = new CreateClassArgs(
            "visitorExporter",
            false,
            false,
            false,
            [],
            [visitCircleOverrideMethod, visitSquareOverrideMethod],
            visitorInterfaceId
        );

        return classService.CreateClass(visitorExportArgs);
    }

    private CreateMethodArgs VisitSquareOverrideMethodArgs(Guid squareShapeId)
    {
        return new CreateMethodArgs(
            "visitSquare",
            VoidGuid,
            "public",
            false,
            false,
            true,
            false,
            false,
            Guid.Empty,
            [],
            [
                new CreateVariableArgs(
                    name: "square",
                    classId: squareShapeId)
            ],
            []
        );
    }

    private CreateMethodArgs VisitCircleOverrideMethodArgs(Guid circleShapeId)
    {
        return new CreateMethodArgs(
            "visitCircle",
            VoidGuid,
            "public",
            false,
            false,
            true,
            false,
            false,
            Guid.Empty,
            [],
            [
                new CreateVariableArgs(
                    name: "circle",
                    classId: circleShapeId)
            ],
            []
        );
    }

    #endregion

    #region VisitorInterface

    private Class CreateVisitorInterface()
    {
        var visitorClassArgs = new CreateClassArgs(
            "visitor",
            true,
            false,
            true,
            [],
            [],
            null
        );
        return classService.CreateClass(visitorClassArgs);
    }

    private Method CreateVisitSquareInterfaceMethod(Guid visitorInterfaceId, Guid squareShapeId)
    {
        var visitSquareInterfaceArgs = new CreateMethodArgs(
            "visitSquare",
            VoidGuid,
            "public",
            true,
            false,
            false,
            false,
            false,
            visitorInterfaceId,
            [],
            [
                new CreateVariableArgs(
                    name: "square",
                    classId: squareShapeId)
            ],
            []
        );

        return methodService.CreateMethod(visitSquareInterfaceArgs);
    }

    private Method CreateVisitCircleInterfaceMethod(Guid visitorInterfaceId)
    {
        var visitCircleInterfaceArgs = new CreateMethodArgs(
            "visitCircle",
            VoidGuid,
            "public",
            true,
            false,
            false,
            false,
            false,
            visitorInterfaceId,
            [],
            [
                new CreateVariableArgs(
                    name: "circle",
                    classId: classService.GetIdByName("circle"))
            ],
            []
        );

        return methodService.CreateMethod(visitCircleInterfaceArgs);
    }

    #endregion

    #region Shapes

    private Class CreateShapeInterface()
    {
        var shapeInterfaceArgs = new CreateClassArgs(
            "shape",
            true,
            false,
            true,
            [],
            [],
            null
        );
        return classService.CreateClass(shapeInterfaceArgs);
    }

    #region Square

    private Class CreateSquareShapeClass(Guid shapeInterfaceId)
    {
        var squareShapeArgs = new CreateClassArgs(
            "square",
            true,
            false,
            true,
            [new CreateAttributeArgs(Int,
                "public",
                Guid.Empty,
                "radius",
                false
            )],
            [],
            shapeInterfaceId
        );

        return classService.CreateClass(squareShapeArgs);
    }

    private CreateMethodArgs CreateAcceptMethodOverrideSquare(Guid visitorInterfaceId, Guid visitSquareAcceptMethodId)
    {
        return new CreateMethodArgs(
            "accept",
            VoidGuid,
            "public",
            false,
            false,
            true,
            false,
            false,
            Guid.Empty,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorInterfaceId)
            ],
            [
                new CreateInvokeMethodArgs(visitSquareAcceptMethodId, "visitor")
            ]
        );
    }

    #endregion

    #region Circle

    private Class CreateCircleShapeClass(Guid shapeInterfaceId)
    {
        var circleShapeArgs = new CreateClassArgs(
            "circle",
            true,
            false,
            true,
            [new CreateAttributeArgs(Int,
                "public",
                Guid.Empty,
                "radius",
                false
                )],
            [],
            shapeInterfaceId
        );

        return classService.CreateClass(circleShapeArgs);
    }

    private CreateMethodArgs CreateAcceptMethodOverrideCircle(Guid visitorInterfaceId, Guid visitCircleAcceptMethodId)
    {
        return new CreateMethodArgs(
            "accept",
            VoidGuid,
            "public",
            false,
            false,
            true,
            false,
            false,
            Guid.Empty,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorInterfaceId)
            ],
            [
                new CreateInvokeMethodArgs(visitCircleAcceptMethodId, "visitor")
            ]
        );
    }

    #endregion

    #endregion

    private Method CreateAcceptMethodInterface(Guid visitorId, Guid shapeInterfaceId)
    {
        var acceptArgs = new CreateMethodArgs(
            "accept",
            VoidGuid,
            "public",
            true,
            false,
            false,
            false,
            false,
            shapeInterfaceId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            []
        );

        return methodService.CreateMethod(acceptArgs);
    }

}
