using Microsoft.AspNetCore.Mvc;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Filter;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/simulator")]
[TypeFilter(typeof(ExceptionFilter))]
public class SimulatorController(IMethodSimulatorService simulatorService) : ControllerBase
{
    [HttpPost]

    public IActionResult SimulateExecution([FromBody] SimulateExecutionArgs args)
    {
        var result = simulatorService.Simulate(args);
        return Ok(result);
    }
}
