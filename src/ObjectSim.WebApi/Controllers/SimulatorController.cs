using Microsoft.AspNetCore.Mvc;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.Filter;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/simulator")]
public class SimulatorController(IMethodSimulatorService simulatorService) : ControllerBase
{

    [HttpPost]
    public IActionResult SimulateExecution([FromBody]CreateSimulateExecutionDtoIn methodExcecution)
    {
        var result = simulatorService.Simulate(methodExcecution.ToArgs());
        return Ok(result);
    }
}
