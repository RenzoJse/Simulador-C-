using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/namespaces")]
public class NamespaceController(INamespaceService service) : ControllerBase
{
    private readonly INamespaceService _service = service;
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _service.GetAll()
            .Select(NamespaceInformationDtoOut.FromEntity)
            .ToList();

        return Ok(result);
    }
    [HttpPost]
    public IActionResult Create([FromBody] CreateNamespaceDtoIn dto)
    {
        _service.Create(dto.ToArgs());
        return Ok();
    }
    [HttpGet("{namespaceId:guid}/descendants")]
    public IActionResult GetAllDescendants([FromRoute] Guid namespaceId)
    {
        var descendants = _service.GetAllDescendants(namespaceId)
            .Select(NamespaceInformationDtoOut.FromEntity)
            .ToList();

        return Ok(descendants);
    }

}
