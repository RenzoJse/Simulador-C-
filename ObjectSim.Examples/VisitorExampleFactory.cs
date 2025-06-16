using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.Examples;

public class VisitorExampleFactory(IClassService classService, IMethodService methodService) : IExampleService
{
    readonly Guid VOID_GUID = Guid.Parse("00000000-0000-0000-0000-000000000005");

    public void CreateExample()
    {
        var shapeInterface = CreateShapeInterface();
        var visitorInterface = CreateVisitorInterface();

        var acceptMethodInterface = CreateAcceptMethodInterface(visitorInterface.Id, shapeInterface.Id);

        var circleShapeClass = CreateCircleShapeClass(shapeInterface.Id);
        var squareShapeClass = CreateSquareShapeClass(shapeInterface.Id);

        var visitCircleInterfaceMethod = CreateVisitCircleInterfaceMethod(visitorInterface.Id, circleShapeClass.Id);
        var visitSquareInterfaceMethod = CreateVisitSquareInterfaceMethod(visitorInterface.Id, squareShapeClass.Id);

        var visitorExportClass = CreateVisitorExportClass(visitorInterface.Id);


        // hasta aca bien





        var circleAcceptOverrideMethod = CreateCircleAcceptOverrideMethod(circleShapeClass.Id, visitorInterface.Id, acceptMethodInterface.Id);
    }

    #region ExportVisitor

    private Class CreateVisitorExportClass(Guid visitorInterfaceId)
    {
        var visitCircleOverrideMethod = VisitCircleOverrideMethodArgs();

        var visitorExportArgs = new CreateClassArgs(
            "visitorExporter",
            false,
            false,
            false,
            [],
            [],
            visitorInterfaceId
        );

        return classService.CreateClass(visitorExportArgs);
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

    private Method CreateVisitCircleInterfaceMethod(Guid visitorInterfaceId, Guid circleShapeId)
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
                    classId: circleShapeId)
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
            "visitor",
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

    private Class createShapeClassArgs()
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
            Guid.Empty,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            []
        );

       var shapeClassArgs = new CreateClassArgs(
            "shape",
            true,
            false,
            true,
            [],
            [
                acceptArgs
            ],
            null
        );

        return classService.CreateClass(shapeClassArgs);
    }
}
