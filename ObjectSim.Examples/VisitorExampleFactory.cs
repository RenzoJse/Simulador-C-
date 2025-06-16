using ObjectSim.IBusinessLogic;
using ObjectSim.Domain.Args;
namespace ObjectSim.Examples;
public  class VisitorExampleFactory(IClassService classService, IMethodService methodService):IExampleService
{
    public void CreateExample()
    {

        
        var voidGuid = Guid.Parse("00000000-0000-0000-0000-000000000005");
        var visitorId = classService.GetIdByName("visitor").Id;
        var circleId = classService.GetIdByName("circle").Id;
        var rectangleId = classService.GetIdByName("rectangle").Id;
        var shapeId = classService.GetIdByName("shape").Id;
        var sizeVisitorId = classService.GetIdByName("sizevisitor").Id;
        var visitRectangleOverrideId = methodService.GetIdByName("visitrectangle").Id;

        var visitCircleArgs = new CreateMethodArgs(
        name: "visitcircle",
        typeId: voidGuid,
        accessibility: "public",
        isAbstract: true,
        isSealed: false,
        isOverride: false,
        isVirtual: false,
        isStatic: false,
        classId: classService.GetIdByName("circle").Id,
        localVariables: [],
        parameters: [
            new CreateVariableArgs(
                name: "circle",
                classId: circleId
            )
        ],
        invokeMethods: []
        );

        var visitRectangleArgs = new CreateMethodArgs(
            name: "visitrectangle",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: null,
            isOverride: null,
            isVirtual: null,
            isStatic: null,
            classId: rectangleId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                            name: "rectangle",
                            classId: rectangleId
                        )
            ],
            invokeMethods: []
            );


        var visitorClassArgs = new CreateClassArgs(
            name: "visitor",
            isAbstract: true,
            isSealed: false,
            isInterface: true,
            attributes: [],
            methods:
            [
                visitCircleArgs,
                visitRectangleArgs
            ],
            parent: null
        );


        var acceptArgs = new CreateMethodArgs(
            name: "accept",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: false,
            isOverride: false,
            isVirtual: false,
            isStatic: false,
            classId: shapeId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                                    name: "visitor",
                                    classId: visitorId)
            ],
            invokeMethods: []
            );


        var shapeClassArgs = new CreateClassArgs(
            name: "shape",
            isAbstract: true,
            isSealed: false,
            isInterface: true,
            attributes: [],
            methods:
            [
                acceptArgs
            ],
            parent: null
        );

        var circleClassArgs = new CreateClassArgs(
            name: "circle",
            isAbstract: false,
            isSealed: false,
            isInterface: false,
            attributes: [],
            methods:
            [
            ],
            parent: shapeId
        );

        var acceptCircleArgs = new CreateMethodArgs(
            name: "accept",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: false,
            isOverride: true,
            isVirtual: false,
            isStatic: false,
            classId: circleId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                name: "visitor",
                classId: visitorId)
            ],
            invokeMethods: []
            );

        var rectangleClassArgs = new CreateClassArgs(
            name: "rectangle",
            isAbstract: false,
            isSealed: false,
            isInterface: false,
            attributes: [],
            methods:
            [
            ],
            parent: shapeId
        );

        var aux = new CreateInvokeMethodArgs(visitRectangleOverrideId, "visitor");
        var acceptRectangleArgs = new CreateMethodArgs(
            name: "Accept",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: false,
            isOverride: true,
            isVirtual: false,
            isStatic: false,
            classId: rectangleId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                                    name: "visitor",
                                    classId: visitorId)
            ],
            invokeMethods: [aux.InvokeMethodId]
            );

        var visitCircleOverrideArgs = new CreateMethodArgs(
            name: "visitCircle",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: false,
            isOverride: true,
            isVirtual: false,
            isStatic: false,
            classId: sizeVisitorId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                name: "visitor",
                classId: visitorId)
            ],
            invokeMethods: []
            );

        var visitRectangleOverrideArgs = new CreateMethodArgs(
            name: "Visitrectangle",
            typeId: voidGuid,
            accessibility: "public",
            isAbstract: true,
            isSealed: false,
            isOverride: true,
            isVirtual: false,
            isStatic: false,
            classId: sizeVisitorId,
            localVariables: [],
            parameters: [
                new CreateVariableArgs(
                name: "visitor",
                classId: visitorId)
            ],
            invokeMethods: []
            );

        var sizeVisitorsArgs = new CreateClassArgs(
            name: "sizevisitor",
            isAbstract: false,
            isSealed: false,
            isInterface: false,
            attributes: [],
            methods:
            [visitCircleOverrideArgs, visitRectangleOverrideArgs
            ],
            parent: visitorId
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
