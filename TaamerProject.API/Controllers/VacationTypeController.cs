using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class VacationTypeController : ControllerBase
    {
        private readonly IVacationTypeService _VacationTypeservice;
        public GlobalShared _globalshared;
        public VacationTypeController(IVacationTypeService vacationTypeservice)
        {
            _VacationTypeservice = vacationTypeservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllVacationsTypes")]
        public IActionResult GetAllVacationsTypes(string SearchText)
        {
            return Ok(_VacationTypeservice.GetAllVacationsTypes(SearchText));
        }

        [HttpGet("GetAllVacationsTypes2")]
        public IActionResult GetAllVacationsTypes2()
        {
            return Ok(_VacationTypeservice.GetAllVacationsTypes(""));
        }

        [HttpPost("SaveVacationType")]
        public IActionResult SaveVacationType(VacationType vacationType)
        {
            var result = _VacationTypeservice.SaveVacationType(vacationType, _globalshared.UserId_G, _globalshared.BranchId_G);          
            return Ok(result);
        }
        [HttpPost("DeleteVacationType")]
        public IActionResult DeleteVacationType(int VacationTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _VacationTypeservice.DeleteVacationType(VacationTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillVacationTypeSelect")]

        public IActionResult FillVacationTypeSelect()
        {
            var vac = _VacationTypeservice.FillVacationTypeSelect().Result.Select(s => new
            {
                Id = s.VacationTypeId,
                Name = s.NameAr
            });
            return Ok(vac);
        }
    }
}
