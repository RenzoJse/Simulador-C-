using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/methods")]
public class MethodController(IMethodService methodService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateMethod(MethodDtoIn createMethodDtoIn)
    {
        var methodInfo = methodService.CreateMethod(createMethodDtoIn.ToArgs());

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

    [HttpGet("{id}")]
    public IActionResult GetMethodById(Guid id)
    {
        var methodInfo = methodService.GetById(id);

        if(methodInfo == null)
        {
            return NotFound($"Method with id {id} not found.");
        }

        var response = new MethodOutModel(methodInfo);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAllMethods()
    {
        var methods = methodService.GetAll();

        var response = methods.Select(m => new MethodOutModel(m)).ToList();

        return Ok(response);
    }


    [HttpPost("{methodId}/parameters")]
    public IActionResult AddParameter(Guid methodId, ParameterDtoIn dto)
    {
        try
        {
            var parameter = methodService.AddParameter(methodId, dto.ToEntity());
            var response = new ParameterOutModel(parameter);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{methodId}/local-variables")]
    public IActionResult AddLocalVariable(Guid methodId, LocalVariableDtoIn dto)
    {
        try
        {
            var localVar = methodService.AddLocalVariable(methodId, dto.ToEntity());
            return Ok(new LocalVariableOutModel(localVar));
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
