using Microsoft.AspNetCore.Mvc;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using ValueType = ObjectSim.Domain.ValueType;

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
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateAttributeDtoIn modelIn)
    {
        var updated = _attributeService.Update(id, new Domain.Attribute
        {
            Id = id,
            Name = modelIn.Name,
            Visibility = Enum.Parse<Domain.Attribute.AttributeVisibility>(modelIn.Visibility),
            ClassId = modelIn.ClassId,
            DataType = modelIn.DataTypeKind switch
            {
                "Value" => ValueType.Create(modelIn.DataTypeName),
                "Reference" => ReferenceType.Create(modelIn.DataTypeName),
                _ => throw new ArgumentException("Invalid DataTypeKind")
            }
        });

        var response = AttributeDtoOut.ToInfo(updated);
        return Ok(response);
    }

}
