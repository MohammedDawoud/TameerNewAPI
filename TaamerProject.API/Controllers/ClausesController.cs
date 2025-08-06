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

    public class ClausesController : ControllerBase
    {
        private readonly IAcc_ClausesService _Acc_ClausesService;
        public GlobalShared _globalshared;

        public ClausesController(IAcc_ClausesService acc_ClausesService)
        {
            _Acc_ClausesService = acc_ClausesService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllClauses")]

        public IActionResult GetAllClauses(string SearchText = "")
        {
            var cate = _Acc_ClausesService.GetAllClauses(SearchText);
            return cate == null ? NotFound() : Ok(cate);
        }
        [HttpPost("SaveClause")]

        public IActionResult SaveClause(Acc_Clauses Clause)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Acc_ClausesService.SaveClause(Clause, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteClause")]

        public IActionResult DeleteClause(int ClauseId)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Acc_ClausesService.DeleteClause(ClauseId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillClausesSelect")]

        public IActionResult FillClausesSelect(string SearchText = "")
        {
            var act = _Acc_ClausesService.GetAllClauses(SearchText).Result.Select(s => new
            {
                Id = s.ClauseId,
                Name = s.NameAr
            });
            return Ok(act);
        }

        [HttpGet("FillClausesSelect2")]

        public IActionResult FillClausesSelect2()
        {
            var act = _Acc_ClausesService.GetAllClauses("").Result.Select(s => new
            {
                Id = s.ClauseId,
                Name = s.NameAr
            });
            return Ok(act);
        }
    }
}
