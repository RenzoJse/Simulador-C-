using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using ObjectSim.WebApi.Filter;

namespace ObjectSim.WebApi.Controllers;
[ApiController]
[Route("api/attributes")]
[TypeFilter(typeof(ExceptionFilter))]
public class AttributeController(IAttributeService attributeService) : ControllerBase
{
    private readonly IAttributeService _attributeService = attributeService;

    [HttpPost]
    public IActionResult Create([FromBody] CreateAttributeDtoIn modelIn)
    {
        var attribute = _attributeService.CreateAttribute(modelIn.ToArgs());
        var response = AttributeDtoOut.ToInfo(attribute);
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
    [HttpGet]
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult GetAll()
    {
        var attributes = _attributeService.GetAll();
        return Ok(attributes);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateAttributeDtoIn modelIn)
    {
        var updated = _attributeService.Update(id, modelIn.ToArgs());
        var response = AttributeDtoOut.ToInfo(updated);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ExceptionFilter))]
    public IActionResult GetById(Guid id)
    {
        var attribute = _attributeService.GetById(id);
        var response = AttributeDtoOut.ToInfo(attribute);
        return Ok(response);
    }

    [HttpGet("by-class/{classId}")]
    public IActionResult GetByClassId(Guid classId)
    {
            var attributes = _attributeService.GetByClassId(classId);
            var response = attributes.Select(AttributeDtoOut.ToInfo).ToList();
            return Ok(response);
    }

}
