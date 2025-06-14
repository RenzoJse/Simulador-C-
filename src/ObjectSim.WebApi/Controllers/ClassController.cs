using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService classService) : ControllerBase
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

    [HttpPatch]
    [Route("{classId:guid}/{methodId:guid}")]
    public IActionResult RemoveMethod([FromRoute] Guid classId, [FromRoute] Guid methodId)
    {
        classService.RemoveMethod(classId, methodId);
        return Ok();
    }

    [HttpPatch]
    [Route("{classId:guid}/{attributeId:guid}")]
    public IActionResult RemoveAttribute([FromRoute] Guid classId, [FromRoute] Guid attributeId)
    {
        classService.RemoveAttribute(classId, attributeId);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetAllClasses()
    {
        var classes = classService.GetAll();

        var response = classes.Select(m => new ClassDtoOut(m)).ToList();

        return Ok(response);
    }

    [HttpPatch("{classId:guid}")]
    public IActionResult UpdateClass([FromRoute] Guid classId, [FromBody] UpdateClassNameDto dto)
    {
        classService.UpdateClass(classId, dto.Name);
        return Ok();
    }

}
