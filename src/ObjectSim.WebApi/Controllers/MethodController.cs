using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using ObjectSim.WebApi.Filter;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/methods")]
public class MethodController(IMethodService methodService) : ControllerBase
{

    [HttpPost]
    public IActionResult CreateMethod(MethodDtoIn createMethodDtoIn)
    {
        var methodInfo = methodService.CreateMethod(createMethodDtoIn.ToArgs());

        var response = new MethodInformationDtoOut(methodInfo);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMethod(Guid id)
    {
        methodService.Delete(id);
        return Ok($"Method with id {id} deleted successfully.");
    }

    [HttpGet("{id}")]
    public IActionResult GetMethodById(Guid id)
    {
        var methodInfo = methodService.GetById(id);

        var response = new MethodInformationDtoOut(methodInfo);

        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAllMethods()
    {
        var methods = methodService.GetAll();

        var response = methods.Select(m => new MethodInformationDtoOut(m)).ToList();

        return Ok(response);
    }

    [HttpPatch("{id:guid}/invokeMethods")]
    public IActionResult AddInvokeMethods([FromRoute]Guid id, List<CreateInvokeMethodDtoIn> invokeMethodDtoIn)
    {
        var invokeMethod = methodService.AddInvokeMethod(id, invokeMethodDtoIn.Select(dto => dto.ToArgs()).ToList());

        var response = new MethodInformationDtoOut(invokeMethod);

        return Ok(response);
    }

}
