using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/simulator")]
public class SimulatorController(IMethodSimulatorService simulatorService) : ControllerBase
{

    [HttpPost]
    public IActionResult SimulateExecution([FromBody] CreateSimulateExecutionDtoIn methodExcecution)
    {
        var result = simulatorService.Simulate(methodExcecution.ToArgs());
        return Ok(result);
    }
}
