using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OfficialDocsExpiredController : ControllerBase
    {
        private readonly IEmployeesService _EmpsService;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public OfficialDocsExpiredController(IEmployeesService empsService, IConfiguration _configuration)
        {
            _EmpsService = empsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetOfficialDocsExpired")]
        public IActionResult GetOfficialDocsExpired(int? DepartmentId)
        {
            var result = _EmpsService.GetOfficialDocsExpired(Con??"", DepartmentId == 0 ? null : DepartmentId);
            return Ok(result);
        }

        [HttpGet("GetOfficialDocsExpired_paging")]
        public IActionResult GetOfficialDocsExpired_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10)
        {
            if (SearchText == null || SearchText == "")
            {
                var offic = _EmpsService.GetOfficialDocsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result;
                var data = GeneratePagination<rptGetOfficialDocsExpiredVM>.ToPagedList(offic.ToList(), page, pageSize);
                var result = new PagedLists<rptGetOfficialDocsExpiredVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                var offic = _EmpsService.GetOfficialDocsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x => x.NameAr.Contains(SearchText) || x.Number.Contains(SearchText) || x.DocSource.Contains(SearchText) || x.Branch.Contains(SearchText) || x.Notes.Contains(SearchText) || SearchText == null || SearchText == "");
                var data = GeneratePagination<rptGetOfficialDocsExpiredVM>.ToPagedList(offic.ToList(), page, pageSize);
                var result = new PagedLists<rptGetOfficialDocsExpiredVM>(data.MetaData, data);
                return Ok(result);
            }
        }
    }
}
