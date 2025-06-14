using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/outputModel")]
public class OutputModelController(IOutputModelTransformerService outputModelService) : ControllerBase
{

    [HttpPost]
    public IActionResult UploadDll([FromForm] IFormFile dllFile)
    {
        outputModelService.UploadDll(dllFile.OpenReadStream(), dllFile.FileName);
        return Ok("DLL uploaded successfully.");
    }

    [HttpGet]
    public IActionResult GetImplementationList()
    {
        var implementations = outputModelService.GetImplementationList();

        var response = implementations.Select(OutputModelNameDtoOut.ToInfo).ToList();

        return Ok(response);
    }

}
