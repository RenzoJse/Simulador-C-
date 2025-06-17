using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.Examples;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.Example.Test;

[TestClass]
public class VisitorExampleTests
{
    [TestMethod]
    public void CreateExample_allDependenciesCalled_expectedBehavior()
    {
        var classServiceMock = new Mock<IClassService>();
        var methodServiceMock = new Mock<IMethodService>();

        var shapeInterface = new Class { Id = Guid.NewGuid() };
        classServiceMock.Setup(x => x.CreateClass(It.Is<CreateClassArgs>(a => a.Name == "shape"))).Returns(shapeInterface);

        var visitorInterface = new Class { Id = Guid.NewGuid() };
        classServiceMock.Setup(x => x.CreateClass(It.Is<CreateClassArgs>(a => a.Name == "visitor"))).Returns(visitorInterface);

        var circleClass = new Class { Id = Guid.NewGuid() };
        var squareClass = new Class { Id = Guid.NewGuid() };
        classServiceMock.Setup(x => x.CreateClass(It.Is<CreateClassArgs>(a => a.Name == "circle"))).Returns(circleClass);
        classServiceMock.Setup(x => x.CreateClass(It.Is<CreateClassArgs>(a => a.Name == "square"))).Returns(squareClass);

        var visitSquareMethod = new Method { Id = Guid.NewGuid(), Name = "visitSquare" };
        var visitCircleMethod = new Method { Id = Guid.NewGuid(), Name = "visitCircle" };
        var visitorExporter = new Class { Id = Guid.NewGuid(), Methods = [visitSquareMethod, visitCircleMethod] };
        classServiceMock.Setup(x => x.CreateClass(It.Is<CreateClassArgs>(a => a.Name == "visitorExporter"))).Returns(visitorExporter);

        classServiceMock.Setup(x => x.GetIdByName("circle")).Returns(circleClass.Id);

        methodServiceMock.Setup(x => x.CreateMethod(It.IsAny<CreateMethodArgs>())).Returns(new Method { Id = Guid.NewGuid() });

        methodServiceMock.Setup(x => x.AddInvokeMethod(It.IsAny<Guid>(), It.IsAny<List<CreateInvokeMethodArgs>>()));

        var factory = new VisitorExample(classServiceMock.Object, methodServiceMock.Object);

        factory.CreateExample();

        classServiceMock.Verify(x => x.CreateClass(It.IsAny<CreateClassArgs>()), Times.Exactly(5));
        methodServiceMock.Verify(x => x.CreateMethod(It.IsAny<CreateMethodArgs>()), Times.Exactly(5));
        methodServiceMock.Verify(x => x.AddInvokeMethod(It.Is<Guid>(id => id == visitSquareMethod.Id), It.IsAny<List<CreateInvokeMethodArgs>>()), Times.Once());
        methodServiceMock.Verify(x => x.AddInvokeMethod(It.Is<Guid>(id => id == visitCircleMethod.Id), It.IsAny<List<CreateInvokeMethodArgs>>()), Times.Once());
    }
}
