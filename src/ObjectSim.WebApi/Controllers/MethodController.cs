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
    public IActionResult CreateMethod(CreateMethodDtoIn createCreateMethodDtoIn)
    {
        var methodInfo = methodService.CreateMethod(createCreateMethodDtoIn.ToArgs());

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

    [HttpPatch("{methodId:guid}/invokeMethods")]
    public IActionResult AddInvokeMethods([FromRoute] Guid methodId, List<CreateInvokeMethodDtoIn> invokeMethodDtoIn)
    {
        var invokeMethod = methodService.AddInvokeMethod(methodId, invokeMethodDtoIn.Select(dto => dto.ToArgs()).ToList());

        var response = new MethodInformationDtoOut(invokeMethod);

        return Ok(response);
    }

}
