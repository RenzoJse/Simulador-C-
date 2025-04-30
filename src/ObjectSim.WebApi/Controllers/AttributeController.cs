using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;
[ApiController]
[Route("api/attributes")]
public class AttributeController(IAttributeService attributeService) : ControllerBase
{
    private readonly IAttributeService _attributeService = attributeService;

    [HttpPost]
    public IActionResult Create([FromBody] CreateAttributeDtoIn modelIn)
    {
        if(modelIn == null)
        {
            return BadRequest();
        }
        var attribute = _attributeService.CreateAttribute(modelIn.ToArgs());

        var response = AttributeDtoOut.ToInfo(attribute);

        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var attributes = _attributeService.GetAll();
        return Ok(attributes);
    }
}
