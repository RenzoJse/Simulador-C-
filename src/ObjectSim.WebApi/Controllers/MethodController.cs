using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

public class MethodController (IMethodService methodService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateMethod(MethodDtoIn createMethodDtoIn)
    {
        var methodInfo = methodService.Create(createMethodDtoIn.ToEntity());

        var response = new MethodOutModel(methodInfo);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMethod(Guid id)
    {
        var deleted = methodService.Delete(id);

        if(!deleted)
        {
            return NotFound($"Method with id {id} not found.");
        }

        return Ok($"Method with id {id} deleted successfully.");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMethod(Guid id, MethodDtoIn updateMethodDtoIn)
    {
        var updatedMethod = methodService.Update(id, updateMethodDtoIn.ToEntity());

        if(updatedMethod == null)
        {
            return NotFound($"Method with id {id} not found.");
        }

        var response = new MethodOutModel(updatedMethod);
        return Ok(response);
    }
}
