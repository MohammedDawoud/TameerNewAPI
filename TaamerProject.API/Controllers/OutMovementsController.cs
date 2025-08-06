using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OutMovementsController : ControllerBase
    {

        private IOutMovementsService _outMovementsservice;
        public GlobalShared _globalshared;
        public OutMovementsController(IOutMovementsService outMovementsservice)
        {
            _outMovementsservice = outMovementsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllOutMovements")]
        public IActionResult GetAllOutMovements(int? TrailingId)
        {            
            return  Ok(_outMovementsservice.GetAllOutMovements(TrailingId) );
        }
        [HttpPost("SaveOutMovements")]
        public IActionResult SaveOutMovements(OutMovements outMovements)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _outMovementsservice.SaveOutMovements(outMovements,_globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpGet("ConfirmOutMovements")]
        public IActionResult ConfirmOutMovements(int? TrailingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _outMovementsservice.ConfirmOutMovements(TrailingId,_globalshared.UserId_G, _globalshared.BranchId_G);
            
            return  Ok(result);
        }
        [HttpPost("DeleteOutMovements")]
        public IActionResult DeleteOutMovements(int MoveId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _outMovementsservice.DeleteOutMovements(MoveId,_globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
    }
}
