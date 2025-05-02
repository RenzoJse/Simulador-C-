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
    /*
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateAttributeDtoIn modelIn)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if(id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }
        var attributeToUpdate = new Domain.Attribute
        {
            Id = id,
            Name = modelIn.Name,
            ClassId = modelIn.ClassId,
            Visibility = Enum.Parse<Domain.Attribute.AttributeVisibility>(modelIn.Visibility),
            DataType = modelIn.DataTypeKind == "Value"
                ? ValueType.Create(modelIn.DataTypeName)
                : ReferenceType.Create(modelIn.DataTypeName)
        };

        var updated = _attributeService.Update(id, attributeToUpdate);
        var response = AttributeDtoOut.ToInfo(updated);
        return Ok(response);
    }*/

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        if(id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            var attribute = _attributeService.GetById(id);
            var response = AttributeDtoOut.ToInfo(attribute);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("by-class/{classId}")]
    public IActionResult GetByClassId(Guid classId)
    {
        if(classId == Guid.Empty)
        {
            return BadRequest("Invalid ClassId.");
        }

        try
        {
            var attributes = _attributeService.GetByClassId(classId);
            var response = attributes.Select(AttributeDtoOut.ToInfo).ToList();
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
