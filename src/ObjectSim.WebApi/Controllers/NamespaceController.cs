using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/namespaces")]
public class NamespaceController(INamespaceService service) : ControllerBase
{
    private readonly INamespaceService _service = service;
    [HttpGet]
    public ActionResult<List<NamespaceInformationDtoOut>> GetAll()
    {
        var result = _service.GetAll()
            .Select(NamespaceInformationDtoOut.FromEntity)
            .ToList();

        return Ok(result);
    }

}
