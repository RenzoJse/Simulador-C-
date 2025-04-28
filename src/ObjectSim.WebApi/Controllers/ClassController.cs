using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService classService)
    : ControllerBase
{

    [HttpPost]
    public IActionResult CreateClass(CreateClassDtoIn createClassDto)
    {
        var classInfo = classService.CreateClass(createClassDto.ToArgs());

        var response = ClassInformationDtoOut.ToInfo(classInfo);

        return Ok(response);
    }

    [HttpGet]
    [Route("{classId:guid}")]
    public IActionResult GetClass([FromRoute] Guid classId)
    {
        var classInfo = classService.GetById(classId);

        var response = ClassInformationDtoOut.ToInfo(classInfo);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{classId:guid}")]
    public IActionResult DeleteClass([FromRoute] Guid classId)
    {
        classService.DeleteClass(classId);
        return Ok();
    }

}
