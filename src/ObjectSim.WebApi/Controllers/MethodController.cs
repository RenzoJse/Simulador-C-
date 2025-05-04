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
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult CreateMethod(MethodDtoIn createMethodDtoIn)
    {
        var methodInfo = methodService.CreateMethod(createMethodDtoIn.ToArgs());

        var response = new MethodInformationDtoOut(methodInfo);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult DeleteMethod(Guid id)
    {
        methodService.Delete(id);
        return Ok($"Method with id {id} deleted successfully.");
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult GetMethodById(Guid id)
    {
        var methodInfo = methodService.GetById(id);

        var response = new MethodInformationDtoOut(methodInfo);

        return Ok(response);
    }

    [HttpGet]
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult GetAllMethods()
    {
        var methods = methodService.GetAll();

        var response = methods.Select(m => new MethodInformationDtoOut(m)).ToList();

        return Ok(response);
    }

}
