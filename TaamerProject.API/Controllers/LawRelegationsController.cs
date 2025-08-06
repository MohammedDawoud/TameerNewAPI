using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LawRelegationsController : ControllerBase
    {

        private ILaw_regulationsService _LawregulationsService;
        public GlobalShared _globalshared;

        public LawRelegationsController( ILaw_regulationsService lawregulationsService)
        {
            _LawregulationsService = lawregulationsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        }


        [HttpGet("GetLaw_Regulations")]
        public IActionResult GetLaw_Regulations()
        {
            var lawrelegations = _LawregulationsService.GetLaw_Regulations();
            return Ok(lawrelegations);

        }


        [HttpPost("saveLateLaw")]
        public IActionResult saveLateLaw(Emp_LateList lateList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _LawregulationsService.saveLateLaw(lateList, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);

        }


        [HttpPost("saveAbsenceLaw")]
        public IActionResult saveAbsenceLaw(Emp_AbsenceList absenceList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _LawregulationsService.saveAbsenceLaw(absenceList, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);

        }


   

    }
    }
