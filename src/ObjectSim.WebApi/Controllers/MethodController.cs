using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.WebApi.Controllers;

public class MethodController (IMethodService methodService) : ControllerBase
{
}
