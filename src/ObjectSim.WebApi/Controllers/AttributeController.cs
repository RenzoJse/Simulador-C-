using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using ObjectSim.WebApi.Filter;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.WebApi.Controllers;

[ApiController]
[Route("api/attributes")]
public class AttributeController(IAttributeService attributeService) : ControllerBase
{

    [HttpPost]
    public IActionResult Create([FromBody] CreateAttributeDtoIn modelIn)
    {
        Attribute attribute = attributeService.CreateAttribute(modelIn.ToArgs());
        var response = AttributeDtoOut.ToInfo(attribute);
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Attribute> attributes = attributeService.GetAll();
        return Ok(attributes);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateAttributeDtoIn modelIn)
    {
        Attribute updated = attributeService.Update(id, modelIn.ToArgs());
        var response = AttributeDtoOut.ToInfo(updated);

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(Guid id)
    {
        Attribute attribute = attributeService.GetById(id);
        var response = AttributeDtoOut.ToInfo(attribute);
        return Ok(response);
    }

    [HttpGet]
    [Route("by-class/{classId}")]
    public IActionResult GetByClassId(Guid classId)
    {
        List<Attribute> attributes = attributeService.GetByClassId(classId);
        var response = attributes.Select(AttributeDtoOut.ToInfo).ToList();
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        var result = attributeService.Delete(id);
        return Ok(result);
    }

}
