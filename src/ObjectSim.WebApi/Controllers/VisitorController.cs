using Microsoft.AspNetCore.Mvc;
using ObjectSim.Examples;

namespace ObjectSim.WebApi.Controllers;
[ApiController]
[Route("api/examples")]
public class VisitorController(IExampleService visitorEx) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateExamples()
    {
        visitorEx.CreateExample();
        return Ok();
    }
}
