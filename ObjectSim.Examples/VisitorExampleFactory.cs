using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.Examples;

public class VisitorExampleFactory(IClassService classService, IMethodService methodService) : IExampleService
{
    readonly Guid VOID_GUID = Guid.Parse("00000000-0000-0000-0000-000000000005");
    readonly Guid TO_STRING_GUID = Guid.Parse("00000000-0000-0000-0000-000000000108");

    public void CreateExample()
    {
        var shapeInterface = CreateShapeInterface();
        var visitorInterface = CreateVisitorInterface();

        var circleShapeClass = CreateCircleShapeClass(shapeInterface.Id);

        var visitCircleInterfaceMethod = CreateVisitCircleInterfaceMethod(visitorInterface.Id);

        var acceptMethodInterface = CreateAcceptMethodInterface(visitorInterface.Id, shapeInterface.Id);

        var circleAcceptMethodOverrideArgs = CreateAcceptMethodOverride(visitorInterface.Id, visitCircleInterfaceMethod.Id);

        var squareShapeClass = CreateSquareShapeClass(shapeInterface.Id);


        var visitSquareInterfaceMethod = CreateVisitSquareInterfaceMethod(visitorInterface.Id, squareShapeClass.Id);

        var visitorExportClass = CreateVisitorExportClass(visitorInterface.Id, circleShapeClass.Id, squareShapeClass.Id);

        //var circleAcceptMethodOverride = CreateAcceptMethodOverride(circleShapeClass.Id, visitorInterface.Id, visitCircleInterfaceMethod.Id);
        // hasta aca bien
        //  falta el de square;
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
            VOID_GUID,
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
            [
                new CreateInvokeMethodArgs(
                    TO_STRING_GUID,
                    "square"
                )
            ]
        );
    }

    private CreateMethodArgs VisitCircleOverrideMethodArgs(Guid circleShapeId)
    {
        return new CreateMethodArgs(
            "visitCircle",
            VOID_GUID,
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
            [
            new CreateInvokeMethodArgs(
                TO_STRING_GUID,
                    "circle"
                )
            ]
        );
    }

    #endregion

    private Method CreateCircleAcceptOverrideMethod(Guid circleShapeId, Guid visitorId, Guid acceptMethodId)
    {
        var circleAcceptArgs = new CreateMethodArgs(
            "accept",
            VOID_GUID,
            "public",
            false,
            false,
            true,
            false,
            false,
            circleShapeId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            [
                new CreateInvokeMethodArgs(acceptMethodId, "visitor")
            ]
        );

        return methodService.CreateMethod(circleAcceptArgs);
    }

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
            VOID_GUID,
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
            VOID_GUID,
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

    private Class CreateSquareShapeClass(Guid shapeInterfaceId)
    {
        var squareShapeArgs = new CreateClassArgs(
            "square",
            true,
            false,
            true,
            [],
            [],
            shapeInterfaceId
        );

        return classService.CreateClass(squareShapeArgs);
    }

    #region Circle

    private Class CreateCircleShapeClass(Guid shapeInterfaceId)
    {
        var circleShapeArgs = new CreateClassArgs(
            "circle",
            true,
            false,
            true,
            [],
            [],
            shapeInterfaceId
        );

        return classService.CreateClass(circleShapeArgs);
    }

    private CreateMethodArgs CreateAcceptMethodOverride(Guid visitorInterfaceId, Guid visitCircleAcceptMethodId)
    {
        return new CreateMethodArgs(
            "accept",
            VOID_GUID,
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
            VOID_GUID,
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
