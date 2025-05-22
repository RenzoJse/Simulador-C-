using Microsoft.AspNetCore.Mvc;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Controllers;
[ApiController]
[Route("api/datatypes")]
public class DataTypeController(IDataTypeService dataTypeService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<DataTypeInformationDtoOut>> GetAll()
    {
        var result = dataTypeService.GetAll()
            .Select(DataTypeInformationDtoOut.ToInfo)
            .ToList();

        return Ok(result);
    }
    [HttpGet]
    [Route("{id}")]
    public ActionResult<DataTypeInformationDtoOut> GetById(Guid id)
    {
        var dt = dataTypeService.GetById(id);
        return Ok(DataTypeInformationDtoOut.ToInfo(dt));
    }
}
