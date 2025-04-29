using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.WebApi.Controllers;
[ApiController]
[Route("api/companies")]
public class AttributeController(IAttributeService attributeService) : ControllerBase
{
    private readonly IAttributeService _attributeService = attributeService;

    [HttpGet]
    public IActionResult GetAll()
    {
        var attributes = _attributeService.GetAll();
        return Ok(attributes);
    }
}
