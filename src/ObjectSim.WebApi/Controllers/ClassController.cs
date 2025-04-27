using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService classService)
    : ControllerBase
{

    [HttpPost]
    public IActionResult CreateClass(CreateClassDtoIn createClassDto)
    {
        return NotImplementedException();
    }

}
