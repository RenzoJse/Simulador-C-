using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.Examples;

public class VisitorExampleFactory(IClassService classService, IMethodService methodService) : IExampleService
{
    public void CreateExample()
    {
        var voidGuid = Guid.Parse("00000000-0000-0000-0000-000000000005");
        Guid visitorId = classService.GetIdByName("visitor").Id;
        Guid circleId = classService.GetIdByName("circle").Id;
        Guid rectangleId = classService.GetIdByName("rectangle").Id;
        Guid shapeId = classService.GetIdByName("shape").Id;
        Guid sizeVisitorId = classService.GetIdByName("sizevisitor").Id;
        Guid visitRectangleOverrideId = methodService.GetIdByName("visitrectangle").Id;

        var visitCircleArgs = new CreateMethodArgs(
            "visitcircle",
            voidGuid,
            "public",
            true,
            false,
            false,
            false,
            false,
            classService.GetIdByName("circle").Id,
            [],
            [
                new CreateVariableArgs(
                    name: "circle",
                    classId: circleId
                )
            ],
            []
        );

        var visitRectangleArgs = new CreateMethodArgs(
            "visitrectangle",
            voidGuid,
            "public",
            true,
            null,
            null,
            null,
            null,
            rectangleId,
            [],
            [
                new CreateVariableArgs(
                    name: "rectangle",
                    classId: rectangleId
                )
            ],
            []
        );


        var visitorClassArgs = new CreateClassArgs(
            "visitor",
            true,
            false,
            true,
            [],
            [
                visitCircleArgs,
                visitRectangleArgs
            ],
            null
        );


        var acceptArgs = new CreateMethodArgs(
            "accept",
            voidGuid,
            "public",
            true,
            false,
            false,
            false,
            false,
            shapeId,
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

        var circleClassArgs = new CreateClassArgs(
            "circle",
            false,
            false,
            false,
            [],
            [
            ],
            shapeId
        );

        var acceptCircleArgs = new CreateMethodArgs(
            "accept",
            voidGuid,
            "public",
            true,
            false,
            true,
            false,
            false,
            circleId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            []
        );

        var rectangleClassArgs = new CreateClassArgs(
            "rectangle",
            false,
            false,
            false,
            [],
            [
            ],
            shapeId
        );

        var aux = new CreateInvokeMethodArgs(visitRectangleOverrideId, "visitor");
        var acceptRectangleArgs = new CreateMethodArgs(
            "Accept",
            voidGuid,
            "public",
            true,
            false,
            true,
            false,
            false,
            rectangleId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            [aux.InvokeMethodId]
        );

        var visitCircleOverrideArgs = new CreateMethodArgs(
            "visitCircle",
            voidGuid,
            "public",
            true,
            false,
            true,
            false,
            false,
            sizeVisitorId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            []
        );

        var visitRectangleOverrideArgs = new CreateMethodArgs(
            "Visitrectangle",
            voidGuid,
            "public",
            true,
            false,
            true,
            false,
            false,
            sizeVisitorId,
            [],
            [
                new CreateVariableArgs(
                    name: "visitor",
                    classId: visitorId)
            ],
            []
        );

        var sizeVisitorsArgs = new CreateClassArgs(
            "sizevisitor",
            false,
            false,
            false,
            [],
            [
                visitCircleOverrideArgs, visitRectangleOverrideArgs
            ],
            visitorId
        );


        methodService.CreateMethod(visitCircleArgs);
        methodService.CreateMethod(visitRectangleArgs);
        methodService.CreateMethod(acceptArgs);
        methodService.CreateMethod(acceptCircleArgs);
        methodService.CreateMethod(acceptRectangleArgs);
        methodService.CreateMethod(visitCircleOverrideArgs);
        methodService.CreateMethod(visitRectangleOverrideArgs);

        classService.CreateClass(circleClassArgs);
        classService.CreateClass(rectangleClassArgs);
        classService.CreateClass(shapeClassArgs);
        classService.CreateClass(visitorClassArgs);
        classService.CreateClass(sizeVisitorsArgs);
    }
}
